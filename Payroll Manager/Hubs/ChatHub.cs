using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Payroll_Manager.Entity;
using Payroll_Manager.Persistence;

namespace Payroll_Manager.Hubs
{
    public class ChatHub : Hub
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContext;

        public ChatHub(
            IHttpContextAccessor httpContext, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _httpContext = httpContext;
            _userManager = userManager;
            _context = context;
        }

        public async override Task OnConnectedAsync()
        {
            var user = await _userManager.GetUserAsync(_httpContext.HttpContext.User);

            if (_context.OnlineList.Where(x => x.UserId == user.Id).Count() > 0)
            {
                _context.OnlineList.RemoveRange(_context.OnlineList.Where(x => x.UserId == user.Id).ToList());
            }
            _context.OnlineList.Add(new OnlineList { UserId = user.Id });

            await _context.SaveChangesAsync();

            await Clients.All.SendAsync("UserConnected", user);
        }

        public async Task SendMessage(string user, string message,DateTime thoigian)
        {
            _context.Chat.Add(new Chat {
                UserId = user,
                Message = message,
                Date = thoigian
            });

            await _context.SaveChangesAsync();

            await Clients.All.SendAsync("ReceiveMessage", await _userManager.FindByIdAsync(user), message);
        }

        public async override Task OnDisconnectedAsync(Exception exception)
        {
            var user = await _userManager.GetUserAsync(_httpContext.HttpContext.User);

            _context.OnlineList.Remove(_context.OnlineList.FirstOrDefault(x => x.UserId == user.Id));

            await _context.SaveChangesAsync();

            await base.OnDisconnectedAsync(exception);
            await Clients.All.SendAsync("UserDisconnected", user);
        }
    }
}

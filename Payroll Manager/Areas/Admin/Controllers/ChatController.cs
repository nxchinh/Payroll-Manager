using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Payroll_Manager.Persistence;
using Payroll_Manager.Areas.Admin.Models;

namespace Payroll_Manager.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class ChatController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public ChatController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public IActionResult Index()
        {
            var model = new ChatViewModel
            {
                UserId = _userManager.GetUserId(User),
                Users = _context.OnlineList.Select(x => new
                {
                    user = _userManager.FindByIdAsync(x.UserId).Result
                }).Select(x => x.user).ToList(),
                Messages = _context.Chat.Select(x => new MessageViewModel
                {
                    user = _userManager.FindByIdAsync(x.UserId).Result,
                    message = x.Message,
                    date = x.Date
                }).OrderBy(x => x.date).ToList()
            };

            return View(model);
        }
    }
}
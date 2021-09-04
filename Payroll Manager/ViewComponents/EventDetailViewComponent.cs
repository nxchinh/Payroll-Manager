using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Payroll_Manager.Controllers;
using Payroll_Manager.Persistence;

namespace Payroll_Manager.ViewComponents
{
    public class EventDetailViewComponent : ViewComponent
    {
        private ApplicationDbContext _context;
        private readonly IStringLocalizer<UserController> _localizer;

        public EventDetailViewComponent(ApplicationDbContext context, IStringLocalizer<UserController> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var id = _context.EventImages.FirstOrDefault()?.EventId;
            var posts = await _context.Events.ToListAsync();
            var eventDefault = posts.FirstOrDefault(x => id != null && x.Id == id.Value);
            return View(eventDefault);
        }
    }
}

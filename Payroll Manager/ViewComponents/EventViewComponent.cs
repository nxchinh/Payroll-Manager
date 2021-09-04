using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Payroll_Manager.Controllers;
using Payroll_Manager.Entity;
using Payroll_Manager.Persistence;

namespace Payroll_Manager.ViewComponents
{
    public class EventViewComponent : ViewComponent
    {
        private ApplicationDbContext _context; private readonly IStringLocalizer<UserController> _localizer;
        private IMemoryCache _cache;
        private readonly TimeSpan _cacheSlidingExpiration = TimeSpan.FromMinutes(5);
        private readonly TimeSpan _cacheAbsoluteExpiration = TimeSpan.FromMinutes(30);

        public EventViewComponent(ApplicationDbContext context, IStringLocalizer<UserController> localizer, IMemoryCache cache)
        {
            _context = context;
            _localizer = localizer;
            _cache = cache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var id = _context.EventImages.FirstOrDefault()?.EventId;
            ViewBag.TenEvent = _context.Events.FirstOrDefault(x => x.Id == id.Value)?.TenSuKien;
            List<EventImage> cacheEntry;
            string cacheKey = "Danh sách hình ảnh event";
            var posts = await _context.EventImages
                .Where(p => p.Event.Active)
                .ToListAsync();
            if (!_cache.TryGetValue(cacheKey, out cacheEntry))
            {
                cacheEntry = posts.ToList();
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(_cacheSlidingExpiration)
                    .SetAbsoluteExpiration(_cacheAbsoluteExpiration);
                _cache.Set(cacheKey, cacheEntry, cacheEntryOptions);
            }

            return View(cacheEntry);
        }
    }
}

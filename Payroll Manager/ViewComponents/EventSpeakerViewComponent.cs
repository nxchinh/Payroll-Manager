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
    public class EventSpeakerViewComponent : ViewComponent
    {
        private ApplicationDbContext _context;
        private readonly IStringLocalizer<UserController> _localizer;
        private IMemoryCache _cache;
        private readonly TimeSpan _cacheSlidingExpiration = TimeSpan.FromMinutes(1);
        private readonly TimeSpan _cacheAbsoluteExpiration = TimeSpan.FromMinutes(3);
        public EventSpeakerViewComponent(ApplicationDbContext context, IStringLocalizer<UserController> localizer, IMemoryCache cache)
        {
            _context = context;
            _localizer = localizer;
            _cache = cache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewData["String1"] = _localizer["EVENT SPEAKERS"];
            ViewData["String2"] = _localizer["Here are some of our speakers"];
            List<Employee> cacheEntry;
            string cacheKey = "Danh sách diễn giả";
            var id = _context.Departments.SingleOrDefault(x => x.Name == "Diễn giả")?.Id;
            var listnhanvien = await _context.Employees.ToListAsync();
            var listdiengia = listnhanvien.Where(x => id != null && x.DepartmentId == id.Value).ToList();
            if (!_cache.TryGetValue(cacheKey, out cacheEntry))
            {
                cacheEntry = listdiengia;
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(_cacheSlidingExpiration)
                    .SetAbsoluteExpiration(_cacheAbsoluteExpiration);
                _cache.Set(cacheKey, cacheEntry, cacheEntryOptions);
            }
            return View(cacheEntry);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Payroll_Manager.Entity.ModelForUser;
using Payroll_Manager.Persistence;

namespace Payroll_Manager.ViewComponents
{
    public class ProductViewComponent: ViewComponent
    {
        private ApplicationDbContext _context;
        private IMemoryCache _cache;
        private readonly TimeSpan _cacheSlidingExpiration = TimeSpan.FromMinutes(5);
        private readonly TimeSpan _cacheAbsoluteExpiration = TimeSpan.FromMinutes(30);
        public ProductViewComponent(ApplicationDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Inventory> cacheEntry;
            string cacheKey = "Danh sách sản phẩm";
            var posts = await _context.Inventories
                .Where(p => p.Category.Active == 1)
                .OrderByDescending(p => p.ProductId)
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

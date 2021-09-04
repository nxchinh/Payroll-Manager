using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Payroll_Manager.Entity.ModelForUser;
using Payroll_Manager.Persistence;

namespace Payroll_Manager.Services.Implementation
{
    public class InventoryService:IInventoryService
    {
        private readonly ApplicationDbContext _context;
        public InventoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public DbSet<Inventory> GetInventories() => _context.Inventories;

        public IEnumerable<Inventory> GetAll() => _context.Inventories.AsNoTracking().OrderBy(emp => emp.NetPrice).Include(i => i.Category);

        public async Task CreateAsync(Inventory inventory)
        {
            await _context.Inventories.AddAsync(inventory);
            await _context.SaveChangesAsync();
        }
        public Inventory GetSingle(Expression<Func<Inventory, bool>> predicate)
        {
            return _context.Set<Inventory>().FirstOrDefault(predicate);
        }

        public IEnumerable<Inventory> Query(Expression<Func<Inventory, bool>> predicate)
        {
            return _context.Set<Inventory>().Where(predicate);
        }
        public Inventory GetById(int inventoryId) => _context.Inventories.FirstOrDefault(predicate: e => e.ProductId == inventoryId);

        public async Task UpdateAsync(Inventory inventory)
        {
            _context.Update(inventory);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int inventoryId)
        {
            var inventory = GetById(inventoryId);
            _context.Remove(inventory);
            await _context.SaveChangesAsync();
        }
        public IEnumerable<SelectListItem> GetAllInventories()
        {
            return GetAll().Where(s => s.Category.Active == 1).Select(add => new SelectListItem()
            {
                Text = add.FullAd,
                Value = add.ProductId.ToString()
            });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Payroll_Manager.Entity.ModelForUser;
using Payroll_Manager.Persistence;

namespace Payroll_Manager.Services.Implementation
{
    public class PurchaseOrderItemService:IPurchaseOrderItem
    {
        private readonly ApplicationDbContext _context;
        public PurchaseOrderItemService(ApplicationDbContext context)
        {
            _context = context;
        }

        public DbSet<PurchaseOrderItem> GetPurchaseOrdersItem()=> _context.PurchaseOrderItems;
        public IEnumerable<PurchaseOrderItem> GetAll()=> _context.PurchaseOrderItems.AsNoTracking().Include(p => p.Inventory).Include(p => p.PurchaseOrder);

        public async Task CreateAsync(PurchaseOrderItem purchaseOrderItem)
        {
            await _context.PurchaseOrderItems.AddAsync(purchaseOrderItem);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(PurchaseOrderItem purchaseOrderItem)
        {
            _context.Entry(purchaseOrderItem).State = EntityState.Detached;
            _context.Update(purchaseOrderItem);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id)
        {
            var purchase = _context.PurchaseOrderItems.FirstOrDefault(e => e.PoItemId == id);
            _context.Update(purchase);
            await _context.SaveChangesAsync();

        }

        public async Task Delete(PurchaseOrderItem purchaseOrderItem)
        {
            _context.Remove(purchaseOrderItem);
            await _context.SaveChangesAsync();
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public IEnumerable<PurchaseOrderItem> Query(Expression<Func<PurchaseOrderItem, bool>> predicate)=> _context.Set<PurchaseOrderItem>().Where(predicate);
    }
}

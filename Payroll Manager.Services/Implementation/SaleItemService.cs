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
    public class SaleItemService:ISaleItem
    {
        private readonly ApplicationDbContext db;
        public SaleItemService(ApplicationDbContext context)
        {
            db = context;
        }

        public DbSet<SaleItem> GetSaleItem() => db.SaleItems;

        public IEnumerable<SaleItem> GetAll() => db.SaleItems.AsNoTracking().Include(s => s.Inventory).Include(s => s.Sale);

        public async Task CreateAsync(SaleItem saleItem)
        {
            await db.SaleItems.AddAsync(saleItem);
            await db.SaveChangesAsync();
        }

        public async Task UpdateAsync(SaleItem saleItem)
        {
            db.SaleItems.Update(saleItem);
            await db.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id)
        {
            var saleItem = db.SaleItems.FirstOrDefault(e => e.SaleItemId == id);
            db.Entry(saleItem).State = EntityState.Modified;
            db.Update(saleItem);
            await db.SaveChangesAsync();
        }

        public async Task Delete(SaleItem saleItem)
        {
            db.Remove(saleItem);
            await db.SaveChangesAsync();
        }

        public async Task Save()
        {
            await db.SaveChangesAsync();
        }

        public IEnumerable<SaleItem> Query(Expression<Func<SaleItem, bool>> predicate) => db.Set<SaleItem>().Where(predicate);
    }
}

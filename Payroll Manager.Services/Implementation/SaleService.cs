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
    public class SaleService: ISaleService
    {
        private readonly ApplicationDbContext db;
        public SaleService(ApplicationDbContext context)
        {
            db = context;
        }

        public DbSet<Sale> GetSales() => db.Sales;

        public IEnumerable<Sale> GetAll() => db.Sales.AsNoTracking().Include(x=>x.Customer).Include(x=>x.Employee).OrderBy(x =>x.TotalSalePrice);

        public async Task CreateAsync(Sale sale)
        {
            await db.Sales.AddAsync(sale);
            await db.SaveChangesAsync();
        }

        public Sale GetById(int sale) => db.Sales.FirstOrDefault(predicate: e => e.SaleId == sale);

        public async Task UpdateAsync(Sale sale)
        {
            db.Update(sale);
            await db.SaveChangesAsync();
        }

        public async Task Delete(int sale)
        {
            var saleItem = db.Sales.FirstOrDefault(e => e.SaleId == sale);
            db.Remove(saleItem);
            await db.SaveChangesAsync();
        }

        public async Task Delete(Sale sale)
        {
            db.Remove(sale);
            await db.SaveChangesAsync();
        }

        public async Task Save()
        {
            await db.SaveChangesAsync();
        }
    }
}

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
    public class PurchaseOrderService:IPurchaseOrderService
    {
        private readonly ApplicationDbContext _context;
        public PurchaseOrderService(ApplicationDbContext context)
        {
            _context = context;
        }
        public DbSet<PurchaseOrder> GetPurchaseOrders() => _context.PurchaseOrders;

        public IEnumerable<PurchaseOrder> GetAll() => _context.PurchaseOrders.AsNoTracking().OrderBy(emp => emp.OrderDate);

        public void CreateAsync(PurchaseOrder purchaseOrder)
        {
            _context.PurchaseOrders.Add(purchaseOrder);
            _context.SaveChanges();
        }

        public PurchaseOrder GetById(int purchaseorderId) => _context.PurchaseOrders.FirstOrDefault(predicate: e => e.PurchaseOrderId == purchaseorderId);

        public PurchaseOrder GetSingle(Expression<Func<PurchaseOrder, bool>> predicate) => _context.Set<PurchaseOrder>().SingleOrDefault(predicate);

        public IEnumerable<PurchaseOrder> Query(Expression<Func<PurchaseOrder, bool>> predicate)=> _context.Set<PurchaseOrder>().Where(predicate);

        public void UpdateAsync(PurchaseOrder purchaseOrder)
        {
            _context.Update(purchaseOrder);
            _context.SaveChanges();
        }

        public void Delete(int purchaseOrder)
        {
            var getpurchaseOrder = GetById(purchaseOrder);
            _context.Remove(getpurchaseOrder);
            _context.SaveChanges();
        }

        public void Delete(PurchaseOrder purchaseOrder)
        {
            _context.Remove(purchaseOrder);
            _context.SaveChanges();
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Payroll_Manager.Entity.ModelForUser;

namespace Payroll_Manager.Services
{
    public interface IPurchaseOrderService
    {
        DbSet<PurchaseOrder> GetPurchaseOrders();
        IEnumerable<PurchaseOrder> GetAll();
        void CreateAsync(PurchaseOrder category);
        PurchaseOrder GetById(int purchaseorderId);
        PurchaseOrder GetSingle(Expression<Func<PurchaseOrder, bool>> predicate);
        IEnumerable<PurchaseOrder> Query(Expression<Func<PurchaseOrder, bool>> predicate);
        void UpdateAsync(PurchaseOrder purchaseOrder);
        void Delete(int purchaseOrder);
        void Delete(PurchaseOrder purchaseOrder);
        Task Save();
    }
}

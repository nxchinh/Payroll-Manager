using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Payroll_Manager.Entity.ModelForUser;

namespace Payroll_Manager.Services
{
    public interface IPurchaseOrderItem
    {
        DbSet<PurchaseOrderItem> GetPurchaseOrdersItem();
        IEnumerable<PurchaseOrderItem> GetAll();
        Task CreateAsync(PurchaseOrderItem purchaseOrderItem);
        Task UpdateAsync(PurchaseOrderItem purchaseOrderItem);
        Task UpdateAsync(int id);
        Task Delete(PurchaseOrderItem purchaseOrderItem);
        Task Save();
        IEnumerable<PurchaseOrderItem> Query(Expression<Func<PurchaseOrderItem, bool>> predicate);
    }
}

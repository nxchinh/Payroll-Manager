using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Payroll_Manager.Entity.ModelForUser;

namespace Payroll_Manager.Services
{
    public interface ISaleItem
    {
        DbSet<SaleItem> GetSaleItem();
        IEnumerable<SaleItem> GetAll();
        Task CreateAsync(SaleItem saleItem);
        Task UpdateAsync(SaleItem saleItem);
        Task UpdateAsync(int id);
        Task Delete(SaleItem saleItem);
        Task Save();
        IEnumerable<SaleItem> Query(Expression<Func<SaleItem, bool>> predicate);
    }
}

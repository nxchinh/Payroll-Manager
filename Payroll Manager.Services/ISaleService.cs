using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Payroll_Manager.Entity.ModelForUser;

namespace Payroll_Manager.Services
{
    public interface ISaleService
    {
        DbSet<Sale> GetSales();
        IEnumerable<Sale> GetAll();
        Task CreateAsync(Sale sale);
        Sale GetById(int sale);
        Task UpdateAsync(Sale sale);
        Task Delete(int sale);
        Task Delete(Sale sale);
        Task Save();
    }
}

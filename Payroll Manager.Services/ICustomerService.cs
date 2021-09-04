using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Payroll_Manager.Entity;
using Payroll_Manager.Entity.ModelForUser;

namespace Payroll_Manager.Services
{
    public interface ICustomerService
    {
        Task CreateAsync(Customer customer);
        Customer GetById(int customerId);
        Task UpdateAsync(Customer customer);
        Task Delete(int customerId);
        IEnumerable<Customer> GetAll();
        DbSet<Customer> GetCustomers();
    }
}

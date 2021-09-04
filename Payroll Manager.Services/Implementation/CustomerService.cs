using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Payroll_Manager.Entity.ModelForUser;
using Payroll_Manager.Persistence;

namespace Payroll_Manager.Services.Implementation
{
    public class CustomerService:ICustomerService
    {
        private readonly ApplicationDbContext _context;

        public CustomerService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task CreateAsync(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
        }

        public Customer GetById(int customerId) => _context.Customers.FirstOrDefault(add => add.CustomerId == customerId);

        public async Task UpdateAsync(Customer customer)
        {
            _context.Update(customer);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int customerId)
        {
            var customer = GetById(customerId);
            _context.Remove(customer);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<Customer> GetAll() => _context.Customers.AsNoTracking().OrderBy(add => add.Active).Include(c => c.Address);
        public DbSet<Customer> GetCustomers() => _context.Customers;
    }
}

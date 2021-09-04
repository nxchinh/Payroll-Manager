using Payroll_Manager.Entity;
using Payroll_Manager.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Payroll_Manager.Services.Implementation
{
    public class EmployeeService : IEmployeeService
    {
        //mediates communication between a controller and repository layer. 
        //The service layer contains business logic. In particular, it contains validation logic.
        // crud operations
        
        private readonly ApplicationDbContext _context;
        private decimal studentLoanAmount;

        public EmployeeService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task CreateAsync(Employee newEmployee)
        {
           await _context.Employees.AddAsync(newEmployee);
           await _context.SaveChangesAsync();
        }

        public DbSet<Employee> GetEmployees() => _context.Employees;

        public async Task Delete(int employeeId)
        {
            var employee = GetById(employeeId);
            _context.Remove(employee);
            await _context.SaveChangesAsync();
        }
                                                     // AsNoTracking is an entity performance technique.
        public IEnumerable<Employee> GetAll() =>_context.Employees.AsNoTracking().OrderBy(emp => emp.DateJoined).Include(e => e.Address).Include(e => e.Department);
        public IEnumerable<Employee> GetAllSearch(string searchid) => _context.Employees.AsNoTracking().OrderBy(emp => emp.EmployeeNo).Where(o => o.FullName.StartsWith(searchid) || o.EmployeeNo.StartsWith(searchid)).Include(k => k.Address);
        public Employee GetById(int employeeId) => _context.Employees.FirstOrDefault(e => e.Id == employeeId);


        public async Task UpdateAsync(Employee employee)
        {
            _context.Update(employee);
           await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id)
        {
            var employee = GetById(id);
            _context.Update(employee);
           await _context.SaveChangesAsync();
        }

        public decimal StudentLoanRepaymentAmount(int id, decimal totalAmount)
        {
            var employee = GetById(id);
            if (employee.StudentLoan == StudentLoan.Yes && totalAmount > 1750 && totalAmount < 2000)
            {
                studentLoanAmount = 15m;            // payback £15
            }
            else if (employee.StudentLoan == StudentLoan.Yes && totalAmount >= 2000 && totalAmount < 2250)
            {
                studentLoanAmount = 38m;
            }
            else if (employee.StudentLoan == StudentLoan.Yes && totalAmount >= 2250 && totalAmount < 2500)
            {
                studentLoanAmount = 60m;
            }
            else if (employee.StudentLoan == StudentLoan.Yes && totalAmount >= 2500)
            {
                studentLoanAmount = 83m;
            }
            else
            {
                studentLoanAmount = 0m;
            }
            return studentLoanAmount;
        }

        public decimal UnionFees(int id)
        {
            var employee = GetById(id);
            var fee = employee.UnionMember == UnionMember.Yes ? 10m : 0m;
            return fee;
        }

        public IEnumerable<SelectListItem> GetAllEmployeesForPayroll()
        {
            return GetAll().Select(emp => new SelectListItem()
            {
                Text = emp.FullName,
                Value = emp.Id.ToString()
            });
        }
    }
}

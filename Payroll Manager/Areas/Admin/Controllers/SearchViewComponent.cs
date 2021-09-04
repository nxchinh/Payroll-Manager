using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Payroll_Manager.Areas.Admin.Models;
using Payroll_Manager.Areas.Admin.Models.VM_Employee;
using Payroll_Manager.Persistence;

namespace Payroll_Manager.Areas.Admin.Controllers
{
    public class SearchViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public SearchViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var employees = _context.Employees.Select(employee => new EmployeeIndexViewModel
            {
                Id = employee.Id,
                AddressId = employee.AddressId,
                EmployeeNo = employee.EmployeeNo,
                ImageUrl = employee.ImageUrl,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                MiddleName = employee.MiddleName,
                Gender = employee.Gender,
                DateJoined = employee.DateJoined,
                Designation = employee.Designation,
            }).ToListAsync();
            return View(await employees);
        }
    }
}

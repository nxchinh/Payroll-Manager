using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Payroll_Manager.Areas.Admin.Models.VM_Employee;
using Payroll_Manager.Entity;
using Payroll_Manager.Entity.ModelForAttendance;
using Payroll_Manager.Persistence;
using Payroll_Manager.Services;

namespace Payroll_Manager.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    [Authorize]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmployeeService _employeeService;

        public UsersController(ApplicationDbContext context, IEmployeeService employeeService)
        {
            _context = context;
            _employeeService = employeeService;
        }
        private bool _disposed = false;

        protected override void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _context.Dispose();
            }

            _disposed = true;
        }

        public new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        // GET: Users
        public IActionResult Index()
        {
            var employees = _employeeService.GetAll().Select(employee => new EmployeeIndexViewModel
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
                Temporary_status = employee.Temporary_status
            }).ToList();
            return View(employees);
        }   
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Index(List<EmployeeIndexViewModel> users, DateTime selectedDate)
        {
            if (DateTime.Compare(DateTime.MinValue, selectedDate) != 0)
            {
                var attendanceList = from x in _context.Attendance select x;
                var c = attendanceList.FirstOrDefault(a => a.Attendance_Date.Equals(selectedDate));
                if (c == null)
                {
                    if (DateTime.Compare(selectedDate, DateTime.Now) < 1)
                    {
                        foreach (var i in users)
                        {
                                var a = i.Temporary_status;
                                var b = i.Id;
                                Attendance attendanceAdd = new Attendance
                                {
                                    Name = _employeeService.GetById(b).FullName,
                                    Attendance_Date = selectedDate,
                                    Attendance_status = a
                                };
                                _context.Add(attendanceAdd);
                                _context.SaveChanges();

                        }
                        return RedirectToAction("Index", "Attendances");
                    }
                    else
                    {
                        ViewBag.Message = "Errordate";
                        var employees = _employeeService.GetAll().Select(employee => new EmployeeIndexViewModel
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
                            Temporary_status = employee.Temporary_status
                        }).ToList();
                        return View(employees);
                    }
                }
                else
                {
                    ViewBag.Message = "Samedate";
                    var employees = _employeeService.GetAll().Select(employee => new EmployeeIndexViewModel
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
                        Temporary_status = employee.Temporary_status
                    }).ToList();
                    return View(employees);
                }


            }
            else
            {
                ViewBag.Message = "nullDate";
                return RedirectToAction("Index");
            }
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users = await _context.Employees
                .SingleOrDefaultAsync(m => m.Id == id);
            if (users == null)
            {
                return NotFound();
            }

            return View(users);
        }

        public async Task<IActionResult> viewEmp()
        {
            return View(await _context.Employees.AsNoTracking().OrderBy(emp => emp.DateJoined).Include(e => e.Address).ToListAsync());
        }
    }
}

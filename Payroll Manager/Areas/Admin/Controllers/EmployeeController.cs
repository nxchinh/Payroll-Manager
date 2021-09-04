using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting.Internal;
using Payroll_Manager.Entity;
using Payroll_Manager.Services;
using Payroll_Manager.Areas.Admin.Models;
using Payroll_Manager.Areas.Admin.Models.VM_Employee;
using Payroll_Manager.Persistence;
using Payroll_Manager.Utilities;
using static Payroll_Manager.Areas.Admin.Models.CachingExtensions;


namespace Payroll_Manager.Areas.Admin.Controllers
{
    // [Authorize]  
    [Authorize]
    [Authorize(Roles = SD.Admin + "," + SD.DepartmentHead + "," + SD.Hr)]
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class EmployeeController : Controller
    {
        private readonly IMemoryCache _cache;
        private readonly IEmployeeService _employeeService;
        private readonly IAddressService _addressService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ApplicationDbContext _context;

        public EmployeeController(IEmployeeService employeeService, IAddressService addressService, IWebHostEnvironment hostingEnvironment, ApplicationDbContext context, IMemoryCache cache)
        {
            _employeeService = employeeService;
            _addressService = addressService;
            _hostingEnvironment = hostingEnvironment;
            _context = context;
            _cache = cache;
        }
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public IActionResult Index(int? pageNumber)
        {
            var employees = _employeeService.GetAll().Select(employee => new EmployeeIndexViewModel
                {
                    Id = employee.Id,
                    AddressId = employee.AddressId,
                    DepartmentId = employee.DepartmentId,
                    EmployeeNo = employee.EmployeeNo,
                    ImageUrl = employee.ImageUrl,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    MiddleName = employee.MiddleName,
                    Gender = employee.Gender,
                    DateJoined = employee.DateJoined,
                    Designation = employee.Designation,
                    department = _context.Departments.FirstOrDefault(x => x.Id == employee.DepartmentId)
                }).ToList();
            int pageSize = 4;
            ViewBag.List = employees;
            return View(EmployeeListPagination<EmployeeIndexViewModel>.Create(employees, pageNumber ?? 1, pageSize));
        }
        private async Task<string> GetSearchResult(string textInput)
        {
            return await _cache.GetOrCreateAsync($"TenNhanVien-{textInput}", result =>
            {
                result.SlidingExpiration = TimeSpan.FromMinutes(1);
                Console.WriteLine("Not cached, creating response.");
                return Task.FromResult(textInput);
            });
        }

        public async Task<IActionResult> SearchView(int? pageNumber, string searchid)
        {
            var result = await GetSearchResult(searchid);
            var employees = _employeeService.GetAllSearch(result).Select(employee => new EmployeeIndexViewModel
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
                DepartmentId = employee.DepartmentId
            }).ToList();
            int pageSize = 4;
            return View(EmployeeListPagination<EmployeeIndexViewModel>.Create(employees, pageNumber ?? 1, pageSize));

        }
  
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Addresses = _addressService.GetAllAddress();
            ViewBag.Departments = await _context.Departments.ToListAsync();
            var model = new EmployeeCreateViewModel();
            return View(model);
        }
        private string UploadedFile(EmployeeCreateViewModel model)
        {
            string uniqueFileName = null;

            if (model.ImageUrl != null)
            {
                var webRootPath = _hostingEnvironment.WebRootPath;
                string uploadsFolder = Path.Combine(webRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + DateTime.UtcNow.ToString("yymmssfff") + "_" + model.ImageUrl.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.ImageUrl.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        [HttpPost] 
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> Create(EmployeeCreateViewModel model)
        {
            ViewBag.Departments = await _context.Departments.ToListAsync();
            ViewBag.Addresses = _addressService.GetAllAddress();
            if (ModelState.IsValid) 
            {
                string uniqueFileName = UploadedFile(model);
                if (uniqueFileName == null)
                {
                    uniqueFileName = "noimage.png";
                }

                var check = _employeeService.GetAll().FirstOrDefault(x => x.EmployeeNo == model.EmployeeNo);
                if (check == null)
                {
                    var employee = new Employee
                    {
                        Id = model.Id,
                        AddressId = model.AddressId,
                        DepartmentId = model.DepartmentId,
                        EmployeeNo = model.EmployeeNo,
                        FirstName = model.FirstName,
                        MiddleName = model.MiddleName,
                        LastName = model.LastName,
                        FullName = model.FullName,
                        Gender = model.Gender,
                        Email = model.Email,
                        DOB = model.DOB,
                        DateJoined = model.DateJoined,
                        NationalInsuranceNo = model.NationalInsuranceNo,
                        PaymentMethod = model.PaymentMethod,
                        StudentLoan = model.StudentLoan,
                        UnionMember = model.UnionMember,
                        Phone = model.Phone,
                        Designation = model.Designation,
                        ImageUrl = uniqueFileName,
                    };
                    await _employeeService.CreateAsync(employee);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("EmployeeNo", "Trùng mã nhân viên");
                    ViewBag.Addresses = _addressService.GetAllAddress();
                    ViewBag.Departments = await _context.Departments.ToListAsync();
                    return View();
                }

            }
            return View();
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Departments = _context.Departments;
            var employee = _employeeService.GetById(id);
            if (employee == null)
            {
                return NotFound();
            }
            var model = new EmployeeEditViewModel()
            {
                Id = employee.Id,
                DepartmentId = employee.DepartmentId,
                Department = employee.Department,
                AddressId = employee.AddressId,
                EmployeeNo = employee.EmployeeNo,
                FirstName = employee.FirstName,
                MiddleName = employee.MiddleName,
                LastName = employee.LastName,
                Gender = employee.Gender,
                Email = employee.Email,
                DOB = employee.DOB,
                DateJoined = employee.DateJoined, 
                NationalInsuranceNo = employee.NationalInsuranceNo,
                PaymentMethod = employee.PaymentMethod,
                StudentLoan = employee.StudentLoan,
                UnionMember = employee.UnionMember,
                Phone = employee.Phone,
                Address = employee.Address,
                Designation = employee.Designation,
                ImageURL = employee.ImageUrl,
                ImageCu = employee.ImageUrl
            };
            ViewBag.Addresses = _addressService.GetAllAddress();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EmployeeEditViewModel model)
        {
            ViewBag.Departments = await _context.Departments.ToListAsync();
            if (ModelState.IsValid)
            {
                var employee = _employeeService.GetById(model.Id);
                if (employee == null)
                {
                    return NotFound();
                }

                var uniqueFileName = "";
                if (model.ImageUrl != null)
                {
                    var webRootPath = _hostingEnvironment.WebRootPath;
                    string uploadsFolder = Path.Combine(webRootPath, "images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + DateTime.UtcNow.ToString("yymmssfff") + "_" + model.ImageUrl.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        model.ImageUrl.CopyTo(fileStream);
                    }
                }
                employee.EmployeeNo = model.EmployeeNo;
                employee.FirstName = model.FirstName;
                employee.LastName = model.LastName;
                employee.MiddleName = model.MiddleName;
                employee.NationalInsuranceNo = model.NationalInsuranceNo;
                employee.Gender = model.Gender;
                employee.Email = model.Email;
                employee.DOB = model.DOB;
                employee.DateJoined = model.DateJoined;
                employee.Phone = model.Phone;
                employee.Designation = model.Designation;
                employee.PaymentMethod = model.PaymentMethod;
                employee.StudentLoan = model.StudentLoan;
                employee.UnionMember = model.UnionMember;
                employee.AddressId = model.AddressId;
                employee.Address = model.Address;
                employee.FullName = model.FullName;
                employee.DepartmentId = model.DepartmentId;
                employee.Department = model.Department;
                employee.ImageUrl = String.IsNullOrEmpty(uniqueFileName) ? model.ImageCu : uniqueFileName;
                await _employeeService.UpdateAsync(employee);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Addresses = _addressService.GetAllAddress();
            return View();
        }
        [ResponseCache(Duration = 120)]
        public IActionResult Detail(int id)
        {
            var employee = _employeeService.GetById(id);
            EmployeeDetailViewModel model = new EmployeeDetailViewModel()
            {
                Id = employee.Id,
                AddressId = employee.AddressId,
                Address = _addressService.GetById(addressId: employee.AddressId),
                EmployeeNo = employee.EmployeeNo,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                MiddleName = employee.MiddleName,
                Gender = employee.Gender,
                DOB = employee.DOB,
                DateJoined = employee.DateJoined,
                Designation = employee.Designation,
                NationalInsuranceNo = employee.NationalInsuranceNo,
                Phone = employee.Phone,
                Email = employee.Email,
                PaymentMethod = employee.PaymentMethod,
                StudentLoan = employee.StudentLoan,
                UnionMember = employee.UnionMember,
                ImageUrl = employee.ImageUrl,
                DepId = employee.DepartmentId,
                Department = _context.Departments.FirstOrDefault(x => x.Id == employee.DepartmentId)
            };
            return View(model);
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var employee = _employeeService.GetById(id);
            if(employee == null)
            {
                return NotFound();
            }
            var model = new EmployeeDeleteViewModel()
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                MiddleName = employee.MiddleName,
                LastName = employee.LastName
            };
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EmployeeDeleteViewModel model)
        {
            await _employeeService.Delete(model.Id);
            return RedirectToAction(nameof(Index));
        }
    }
}
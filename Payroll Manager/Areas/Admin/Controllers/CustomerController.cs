using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Payroll_Manager.Areas.Admin.Models;
using Payroll_Manager.Areas.Admin.Models.VM_Customer;
using Payroll_Manager.Entity.ModelForUser;
using Payroll_Manager.Persistence;
using Payroll_Manager.Services;
using Payroll_Manager.Services.Implementation;

namespace Payroll_Manager.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class CustomerController : Controller
    {
        private readonly ICustomerService customerService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAddressService _addressService;
        private ApplicationDbContext _context;

        public CustomerController(UserManager<ApplicationUser> userManager, ICustomerService iCustomerService, IAddressService addressService, ApplicationDbContext context)
        {
            _userManager = userManager;
            customerService = iCustomerService;
            _addressService = addressService;
            _context = context;
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
        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
        public ActionResult LowPermission()
        {
            return View();
        }
        // GET: Customers
        public IActionResult Index(string option, string search, int? pageNumber)
        {

            if (User.FindFirstValue(ClaimTypes.NameIdentifier) == null)
            {
                return RedirectToAction("LowPermission");
            }

            var customerlist = customerService.GetAll().Select(cus => new CustomerIndexViewModel
            {
                CustomerId = cus.CustomerId,
                Active = cus.Active,
                Email = cus.Email,
                FullName = cus.FullName,
                PhoneNum = cus.PhoneNum,
                AddressId = cus.AddressId
            }).ToList();
            if (search != null)
            {
                pageNumber = 1;
            }
            int pageSize = 4;
            switch (option)
            {
                case "CustomerID":
                    return View(PaginatedList<CustomerIndexViewModel>.Create(customerlist.Where(i => i.CustomerId.ToString() == search || search == null).ToList(), pageNumber ?? 1, pageSize));
                case "CustomerPhone":
                    return View(PaginatedList<CustomerIndexViewModel>.Create(customerlist.Where(i => i.PhoneNum.ToString() == search || search == null).ToList(), pageNumber ?? 1, pageSize));
                case "CustName":
                    return View(PaginatedList<CustomerIndexViewModel>.Create(customerlist.Where(i => i.FullName.ToString() == search || search == null).ToList(), pageNumber ?? 1, pageSize));
                default:
                    return View(PaginatedList<CustomerIndexViewModel>.Create(customerlist, pageNumber ?? 1, pageSize));
            }

        }

        // GET: Customers/Details/5
        [ResponseCache(Duration = 120)]
        public IActionResult Details(int id)
        {
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) == null)
            {
                return RedirectToAction("LowPermission");
            }
            var cus = customerService.GetById(id);
            if (cus == null)
            {
                return NotFound();
            }
            CustomerDetailViewModel model = new CustomerDetailViewModel
            {
                CustomerId = cus.CustomerId,
                Active = cus.Active,
                Email = cus.Email,
                FirstName = cus.FirstName,
                LastName = cus.LastName,
                PhoneNum = cus.PhoneNum,
                AddressId = cus.AddressId,
                Address = cus.Address
            };
            return View(model);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) == null)
            {
                return RedirectToAction("LowPermission");
            }
            ViewBag.Addresses = _addressService.GetAllAddress();
            var model = new CustomerCreateViewModel();
            return View(model);
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustomerCreateViewModel cus)
        {
            if (ModelState.IsValid)
            {
                var customer = new Customer()
                {
                    CustomerId = cus.CustomerId,
                    Active = 1,
                    Email = cus.Email,
                    FirstName = cus.FirstName,
                    LastName = cus.LastName,
                    FullName = cus.FullName,
                    PhoneNum = cus.PhoneNum,
                    AddressId = cus.AddressId,
                };

                await customerService.CreateAsync(customer);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Addresses = _addressService.GetAllAddress();
            return View();
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) == null)
            {
                return RedirectToAction("LowPermission");
            }
            var cus = customerService.GetById(id);
            if (cus == null)
            {
                return NotFound();
            }
            var model = new CustomerCreateViewModel()
            {
                CustomerId = cus.CustomerId,
                Active = cus.Active,
                Email = cus.Email,
                FirstName = cus.FirstName,
                LastName = cus.LastName,
                PhoneNum = cus.PhoneNum,
                AddressId = cus.AddressId,
            };
            ViewBag.Addresses = _addressService.GetAllAddress();
            return View(model);
        }

        public async Task<IActionResult> Edit(CustomerCreateViewModel cus)
        {
            if (ModelState.IsValid)
            {
                var customer = customerService.GetById(cus.CustomerId);
                if (customer == null)
                {
                    return NotFound();
                }
                customer.CustomerId = cus.CustomerId;
                customer.Active = cus.Active;
                customer.Email = cus.Email;
                customer.FirstName = cus.FirstName;
                customer.LastName = cus.LastName;
                customer.PhoneNum = cus.PhoneNum;
                customer.AddressId = cus.AddressId;
                await customerService.UpdateAsync(customer);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Addresses = _addressService.GetAllAddress();
            return View();
        }
        [ActionName("Restore")]
        public async Task<IActionResult> RestoreConfirmed(int id)
        {
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) == null)
            {
                return RedirectToAction("LowPermission");
            }
            var customer = customerService.GetById(id);
            customer.Active = 1;
            await customerService.UpdateAsync(customer);
            return RedirectToAction("Index");
        }
        // POST: Customers/Delete/5
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) == null)
            {
                return RedirectToAction("LowPermission");
            }
            var customer = customerService.GetById(id);
            customer.Active = 0;
            await customerService.UpdateAsync(customer);
            return RedirectToAction("Index");
        }
    }
}

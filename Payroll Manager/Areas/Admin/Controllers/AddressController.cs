using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Payroll_Manager.Areas.Admin.Models.VM_Address;
using Payroll_Manager.Entity;
using Payroll_Manager.Persistence;
using Payroll_Manager.Services;

namespace Payroll_Manager.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class AddressController : Controller
    {
        private readonly IAddressService _addressService;
        private ApplicationDbContext _context;

        public AddressController(IAddressService addressService, ApplicationDbContext context)
        {
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
        public IActionResult Index(int id)
        {
            IEnumerable<Address> adds = _addressService.GetAll();
            AddressIndexViewModel vm = new AddressIndexViewModel
            {
                Addresses = adds
            };
            return View(vm);
        }
        public ActionResult CreateRedirect()
        {
            return View();
        }
        // GET: /<controller>/
        [HttpGet]
        public IActionResult Add()
        {
            AddressAddViewModel vm = new AddressAddViewModel();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddressAddViewModel vm)
        {
            Guid g = Guid.NewGuid();
            string guidString = Convert.ToBase64String(g.ToByteArray());
            guidString = guidString.Replace("=", "");
            guidString = guidString.Replace("+", "");
            //Guid.NewGuid().ToString()
            if (ModelState.IsValid)
            {
                Address add = new Address
                {
                    Id = vm.Id,
                    AId = guidString,
                    StreetAddress = vm.StreetAddress,
                    AptNumber = vm.AptNumber,
                    City = vm.City,
                    ZipCode = vm.ZipCode,
                    State = vm.State
                };

                await _addressService.CreateAsync(add);
                return RedirectToAction("CreateRedirect");
            }
            return RedirectToAction("Index", "Address");
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            Address add = _addressService.GetById(id);

            AddressAddViewModel vm = new AddressAddViewModel
            {
                Id = add.Id,
                AId = add.AId,
                StreetAddress = add.StreetAddress,
                AptNumber = add.AptNumber,
                City = add.City,
                ZipCode = add.ZipCode,
                State = add.State
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(AddressAddViewModel vm)
        {
            Guid g = Guid.NewGuid();
            string guidString = Convert.ToBase64String(g.ToByteArray());
            guidString = guidString.Replace("=", "");
            guidString = guidString.Replace("+", "");
            if (ModelState.IsValid)
            {
                Address updatedAddress = new Address
                {
                    Id = vm.Id,
                    AId=guidString,
                    StreetAddress = vm.StreetAddress,
                    AptNumber = vm.AptNumber,
                    City = vm.City,
                    ZipCode = vm.ZipCode,
                    State = vm.State
                };
                await _addressService.UpdateAsync(updatedAddress);
                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }

        public async Task<IActionResult> Delete(int id)
        {
            Address add = _addressService.GetById(id);
            await _addressService.Delete(add.Id);
            return RedirectToAction(nameof(Index));
        }
    }
}
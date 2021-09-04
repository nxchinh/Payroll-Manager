using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Payroll_Manager.Areas.Admin.Models.VM_DoanhNghiep;
using Payroll_Manager.Entity;
using Payroll_Manager.Persistence;

namespace Payroll_Manager.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class DepartmentController : Controller
    {
        private readonly ApplicationDbContext _context;
        public DepartmentController(ApplicationDbContext dbContext)
        {
            _context = dbContext;
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
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var data = await _context.Departments.Include(x => x.Company).ToListAsync();
            _context.Departments.Include(x => x.Company);
            return View(data);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {

            var companies = await _context.Companies.ToListAsync();
            ViewBag.Company = companies;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DepartmentViewModel model)
        {
            var companies = await _context.Companies.ToListAsync();
            ViewBag.Company = companies;

            if (ModelState.IsValid)
            {
                Department department = new Department()
                {
                    Id = model.Id,
                    Name = model.Name,
                    CompanyId = model.CompanyId
                };
                await _context.Departments.AddAsync(department);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(List));
            }

            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.Company = await _context.Companies.ToListAsync();

            var data = await _context.Departments.FindAsync(id);
            if (data == null)
            {
                return Redirect("/Home/Error");
            }

            DepartmentViewModel department = new DepartmentViewModel()
            {
                Id = data.Id,
                Name = data.Name,
                CompanyId = data.CompanyId
            };
            

            return View(department);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DepartmentViewModel model)
        {
            var companies = await _context.Companies.ToListAsync();
            ViewBag.Company = companies;

            if (ModelState.IsValid)
            {
                Department department = new Department()
                {
                    Id = model.Id,
                    Name = model.Name,
                    CompanyId = model.CompanyId
                };
                _context.Departments.Update(department);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(List));
            }

            return View();
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var department = await _context.Departments.Where(x => x.Id == id)
                                   .FirstOrDefaultAsync();
            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(List));
        }
    }
}
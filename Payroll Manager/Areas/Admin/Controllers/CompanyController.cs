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
    public class CompanyController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public CompanyController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private bool _disposed = false;

        protected override void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _dbContext.Dispose();
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
            var data = await _dbContext.Companies.ToListAsync();
            return View(data);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string name)
        {
            Company currentCompany = await _dbContext.Companies.Where(x => x.Name == name)
                                                    .FirstOrDefaultAsync();

            if(name == null)
            {
                ModelState.AddModelError("Name", "Tên không thể để trống");
            }
            else
            {
                if (currentCompany == null)
                {
                    await _dbContext.Companies.AddAsync(new Company
                    {
                        Name = name
                    });
                    await _dbContext.SaveChangesAsync();
                    return RedirectToAction(nameof(List));
                }
                else
                {
                    ModelState.AddModelError("Name", "Tên này đã tồn tại");
                }
            }
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var data = await _dbContext.Companies.FindAsync(id);
            if (data == null)
            {
                return Redirect("/Home/Error");
            }
            CompanyViewModel model = new CompanyViewModel()
            {
                Id = data.Id,
                Name = data.Name
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CompanyViewModel model)
        {
            if (ModelState.IsValid)
            {
                Company company = new Company()
                {
                    Id = model.Id,
                    Name = model.Name
                };
                _dbContext.Companies.Update(company);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(List));
            }
            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var company = await _dbContext.Companies.Where(x => x.Id == id)
                                   .FirstOrDefaultAsync();
            _dbContext.Companies.Remove(company);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(List));
        }

    }
}
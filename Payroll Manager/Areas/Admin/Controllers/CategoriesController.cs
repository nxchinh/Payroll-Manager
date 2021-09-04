using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Payroll_Manager.Areas.Admin.Models.VM_Category;
using Payroll_Manager.Entity.ModelForUser;
using Payroll_Manager.Persistence;
using Payroll_Manager.Services;

namespace Payroll_Manager.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IInventoryService _inventoryService;
        private ApplicationDbContext _context;

        public CategoriesController(ICategoryService categoryService, IInventoryService inventoryService, ApplicationDbContext context)
        {
            _categoryService = categoryService;
            _inventoryService = inventoryService;
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
        public IActionResult Index()
        {
            IEnumerable<Category> cats = _categoryService.GetAll();
            var activeCategories = new List<Category>();
            foreach (Category c in _categoryService.GetAll().ToList())
            {
                activeCategories.Add(c);
            }
            CategoryIndexViewModel vm = new CategoryIndexViewModel
            {
                Total = activeCategories.Count(),
                Categories = activeCategories
            };
            return View(vm);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CategoryCreateViewModel vm)
        {
            if (ModelState.IsValid)
            {
                Category existingCategory = _categoryService.GetSingle(c=>c.CategoryName == vm.CategoryName);
                if (existingCategory == null)
                {
                    Category cat = new Category
                    {
                        CategoryName = vm.CategoryName,
                        Active = vm.Active
                    };
                    await _categoryService.CreateAsync(cat);

                    return RedirectToAction("Index", "Categories");
                }
                else
                {
                    ViewBag.MyMessage = "Category name exists. please change the name";
                    return View(vm);
                }
            }
            return View(vm);
        }
        public IActionResult Details(int id)
        {
            Category cat = _categoryService.GetById(id);
            IEnumerable<Inventory> inventoriesList = _inventoryService.Query(p => p.CategoryId == id);
            var inventories = inventoriesList.ToList();
            CategoryDetailViewModel vm = new CategoryDetailViewModel
            {
                Total = inventories.Count(),
                CategoryName = cat.CategoryName,
                Inventories = inventories.ToList(),
                CategoryId = cat.CategoryId,
                Active = cat.Active
            };
            return View(vm);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            Category cat = _categoryService.GetSingle(c => c.CategoryId == id);
            await _categoryService.Delete(cat.CategoryId);
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Change(int id)
        {
            Category cat = _categoryService.GetSingle(c => c.CategoryId == id);
            if (cat.Active == 1)
            {
                cat.Active = 0;
            }
            else
            {
                cat.Active = 1;
            }
            await _categoryService.UpdateAsync(cat);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Search()
        {
            var vm = new CategorySearchViewModel();
            var categories = _categoryService.GetAll();
            vm.Categories = categories;
            return View(vm);
        }

        [HttpPost]
        public IActionResult Search(CategorySearchViewModel vm)
        {
            IEnumerable<Inventory> inventories = _inventoryService.GetAll().Where(c => c.CategoryId == vm.CategoryId);
            if (vm.MaxPrice > 0)
            {
                inventories = inventories.Where(h => h.NetPrice < vm.MaxPrice || h.SalePrice < vm.MaxPrice);
            }
            if (vm.MinPrice > 0)
            {
                inventories = inventories.Where(h => h.NetPrice > vm.MinPrice || h.SalePrice > vm.MinPrice);
            }
            vm.Inventories = inventories;
            var categories = _categoryService.GetAll();
            vm.Categories = categories;

            return View(vm);
        }

    }
}
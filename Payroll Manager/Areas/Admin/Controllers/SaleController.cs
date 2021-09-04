using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Payroll_Manager.Areas.Admin.Models.VM_Sale;
using Payroll_Manager.Entity.ModelForUser;
using Payroll_Manager.Persistence;
using Payroll_Manager.Services;

namespace Payroll_Manager.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class SaleController : Controller
    {
        private readonly ISaleService iSaleService;
        private readonly ISaleItem iSaleItem;
        private readonly IInventoryService inventoryService;
        private readonly ICustomerService iCustomerService;
        private readonly IEmployeeService iEmployeeService; 
        private readonly UserManager<ApplicationUser> _userManager;
        //private ICacheProvider _cache;
        private DateTime _lastUpdated;


        public SaleController(ISaleService iSaleService, ISaleItem iSaleItem, ICustomerService iCustomerService, IEmployeeService iEmployeeService, UserManager<ApplicationUser> userManager, IInventoryService inventoryService)
        {
            this.iSaleService = iSaleService;
            this.iSaleItem = iSaleItem;
            this.iCustomerService = iCustomerService;
            this.iEmployeeService = iEmployeeService;
            _userManager = userManager;
            this.inventoryService = inventoryService;

        }
        public class SoWithItems
        {
            public IEnumerable<SaleItem> ItemList { get; private set; }
            public Sale S { get; private set; }

            public SoWithItems(Sale x, IEnumerable<SaleItem> y)
            {
                S = x;
                ItemList = y;
            }
        }

        public SoWithItems GetOrderWithItems(int givenId)
        {
            var ansSo = iSaleService.GetSales().SingleOrDefault(x => x.SaleId == givenId);
            var ansList = iSaleItem.GetAll().Where(x => x.SaleId == givenId);
            int currLineItem = 0;
            var saleItems = ansList.ToList();
            foreach (var each in saleItems)
            {
                var a = inventoryService.GetById(each.ProductId).SalePrice;
                double currLineCost = each.Quantity * (double)a;
                currLineItem += each.Quantity;
                each.TotalSiPrice = currLineCost;
                each.TotalSI = currLineItem;
                each.Returned = 0;
            }
            iSaleService.Save();
            iSaleItem.Save();
            var ans = new SoWithItems(ansSo, saleItems);
            SoTotalSet(ans);
            iSaleService.Save();
            iSaleItem.Save();
            return ans;
        }

        public void SoTotalSet(SoWithItems x)
        {
            var allItems = x.ItemList;
            double ans = 0;
            int items = 0;
            foreach (var line in allItems)
            {
                ans += line.TotalSiPrice;
                items = line.TotalSI;
            }
            x.S.TotalSalePrice = ans;
            x.S.TotalSaleItems = items;
            x.S.Status = 0;
            return;
        }

        public double GetTotalSalePrice(int id)
        {
            SoWithItems x = GetOrderWithItems(id);
            return x.S.TotalSalePrice;
        }

        public int GetTotalSaleItems(int id)
        {
            SoWithItems x = GetOrderWithItems(id);
            return x.S.TotalSaleItems;
        }

        public string GetStatus(int id)
        {
            SoWithItems x = GetOrderWithItems(id);
            if (!x.ItemList.Any())
            {
                return "SOLD";
            }
            if (x.ItemList.First().Returned == 0)
            {
                return "SOLD";
            }
            if (x.ItemList.First().Returned == 1)
            {
                return "RETURNED";
            }

            return "RETURNED";

        }
        [HttpGet]
        [ResponseCache(VaryByHeader = "option;search", Duration = 30)]
        public IActionResult TransactionLookup(string option, string search)
        {
            List<SoWithItems> soListComplete = new List<SoWithItems>();
            List<Sale> soList = iSaleService.GetSales().ToList();
            foreach (var item in soList)
            {
                int currId = item.SaleId;
                SoWithItems currItem = GetOrderWithItems(currId);
                soListComplete.Add(currItem);
            }

            foreach (var variable in soListComplete)
            {
                if (iSaleItem != null)
                {
                    var ansList = iSaleItem.GetAll().FirstOrDefault(x => x.SaleId == variable.S.SaleId)?.Returned;
                    if (ansList == null)
                    {
                        iSaleService.Delete(variable.S);
                    }
                    variable.S.HoatDong = ansList == 1 ? "RETURNED" : GetStatus(variable.S.SaleId);
                }
                variable.S.items = GetTotalSaleItems(variable.S.SaleId);
                variable.S.price = GetTotalSalePrice(variable.S.SaleId);
                variable.S.TenNhanVien = iEmployeeService.GetById(variable.S.EmployeeId).FullName;
                variable.S.TenKhachHang = iCustomerService.GetById((int)variable.S.CustomerId).FullName;
            }
            //string cacheKey = "Searchfields";
            //bool cacheExists = _cache.TryGetItem(cacheKey, _lastUpdated, out List<SoWithItems> fields);
            //if (!cacheExists)
            //{
            //    fields = soListComplete;
            //    _cache.AddItem(cacheKey, _lastUpdated, fields);
            //}

            if (option == "Tên nhân viên")
            {
                if (search == null)
                {
                    return View(soListComplete.OrderByDescending(x => x.S.SaleDateString).ThenBy(q => q.S.price).ThenBy(m => m.S.items).Take(15).ToList());
                }
                var searchcus = soListComplete.Where(x => x.S.TenNhanVien.ToLower().ToString().Contains(search.ToLower())).ToList();
                if (searchcus.Count == 0)
                {
                    return View(new List<SoWithItems>());

                }
                return View(searchcus);
            }
            else if (option == "Tên khách hàng")
            {
                if(search == null)
                {
                    return View(soListComplete.OrderByDescending(x => x.S.SaleDateString).ThenBy(q => q.S.price).ThenBy(m => m.S.items).Take(15).ToList());
                }

                var searchcus = soListComplete.Where(x => x.S.TenKhachHang.ToLower().ToString().Contains(search.ToLower())).ToList();
                if (searchcus.Count != 0)
                {
                    return View(searchcus);
                }
                return View(new List<SoWithItems>());
            }
            else
            {
                return View(soListComplete.OrderByDescending(x => x.S.SaleDateString).ThenBy(q => q.S.price).ThenBy(m => m.S.items).Take(15).ToList());
            }
        }

        public IActionResult Cancel(int? id)
        {
            SoWithItems saleOrder = GetOrderWithItems((int)id);
            foreach (var item in saleOrder.ItemList)
            {
                iSaleItem.Delete(item);
            }
            iSaleService.Delete(saleOrder.S);
            iSaleItem.Save();
            iSaleService.Save();

            return RedirectToAction("Index");
        }
        public IActionResult Index()
        {
            return View();
        }
        // GET: Sales/NewSale
        public IActionResult NewSale()
        {
            ViewBag.CustomerID = new SelectList(iCustomerService.GetCustomers(), "CustomerId", "FullName");
            ViewBag.EmployeeID = new SelectList(iEmployeeService.GetEmployees().Include(x => x.Department).Where(x => !x.Department.Name.Contains("Diễn giả")), "Id", "DropdownStr");
            var model = new SaleCreateViewModel();
            return View(model);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> NewSale(SaleCreateViewModel model)
        {
            var session = HttpContext.Session;
            if (ModelState.IsValid)
            {
                Sale sale = new Sale()
                {
                    SaleDate = DateTime.Now,
                    SaleId = model.SaleId,
                    CustomerId = model.CustomerId,
                    EmployeeId = model.EmployeeId
                };
                await iSaleService.CreateAsync(sale);
                session.SetInt32("Current CustomerID", (int)sale.CustomerId);
                session.SetInt32("Current SaleID", sale.SaleId);

                return RedirectToAction("Create","SaleItem");
            }

            ViewBag.CustomerID = new SelectList(iCustomerService.GetCustomers(), "CustomerId", "FullName", model.CustomerId);
            ViewBag.EmployeeID = new SelectList(iEmployeeService.GetEmployees().Where(x => !x.Designation.Contains("Speaker")), "Id", "DropdownStr", model.EmployeeId);
            return View(model);
        }

        public async Task<IActionResult> ReturnTransaction(int? id)
        {
            SoWithItems saleOrder = GetOrderWithItems((int)id);
            ApplicationUser applicationUser = await _userManager.GetUserAsync(User);
            string userEmail = applicationUser?.Email;
            foreach (var item in saleOrder.ItemList)
            {
                var i = iSaleItem.GetSaleItem().FirstOrDefault(x => x.SaleItemId == item.SaleItemId);
                inventoryService.GetById(item.ProductId).RestQuantity += i.Quantity;
                i.Quantity = 0;
                i.TotalSI = 0;
                i.TotalSiPrice = 0;
                i.Returned = 1;
                i.UpdatedBy = userEmail;
                i.UpdatedDate = DateTime.Now;
                await iSaleItem.UpdateAsync(i);
            }
            await iSaleService.Save();
            await iSaleItem.Save();
            return RedirectToAction("TransactionLookup");
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Sale sale = iSaleService.GetById((int) id);

            if (sale == null)
            {
                return NotFound();
            }

            var x = GetOrderWithItems((int) id);
            ViewBag.TenNhanVien = iEmployeeService.GetById(x.S.EmployeeId).FullName;
            ViewBag.TenKhachHang = iCustomerService.GetById((int)x.S.CustomerId).FullName;
            var ansList = iSaleItem.GetAll().FirstOrDefault(y => y.SaleId == x.S.SaleId)?.Returned;
            if (ansList == null)
            {
                iSaleService.Delete(x.S);
            }
            ViewBag.HoatDong = ansList == 1 ? "1" : "0";
            var session = HttpContext.Session;
            session.SetString("SaleId",x.S.SaleId.ToString());
            return View(x);
        }

    }
}

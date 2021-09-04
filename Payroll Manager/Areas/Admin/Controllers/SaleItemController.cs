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
using Payroll_Manager.Entity.ModelForUser;
using Payroll_Manager.Persistence;
using Payroll_Manager.Services;

namespace Payroll_Manager.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class SaleItemController : Controller
    {
        private readonly ISaleItem iSaleItem;
        private readonly IInventoryService inventoryService;
        private readonly ISaleService iSaleService; private readonly UserManager<ApplicationUser> _userManager;

        public SaleItemController(ISaleItem iSaleItem, IInventoryService inventoryService, ISaleService iSaleService, UserManager<ApplicationUser> userManager)
        {
            this.iSaleItem = iSaleItem;
            this.inventoryService = inventoryService;
            this.iSaleService = iSaleService;
            _userManager = userManager;
        }
        public IActionResult Index()
        {

            var saleItems = iSaleItem.GetAll();

            return View(saleItems.ToList());
        }
        public IActionResult CheckOut()
        {
            var allSaleItems = iSaleItem.GetSaleItem().ToList();
            foreach (SaleItem saleItem in allSaleItems)
            {
                saleItem.TotalSiPrice += saleItem.Quantity * (double)inventoryService.GetById(saleItem.ProductId).Quantity;
                saleItem.TotalSI += saleItem.Quantity;
                iSaleService.GetById(saleItem.SaleId).TotalSalePrice += saleItem.TotalSiPrice;
                iSaleService.GetById(saleItem.SaleId).TotalSaleItems += saleItem.TotalSI;
            }
            return View(allSaleItems);
        }
        public async Task<IActionResult> Create()
        {
            var allSaleItems = iSaleItem.GetSaleItem().ToList();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            var role = await _userManager.GetRolesAsync(user);
            ViewBag.userEmail = userId;
            ViewBag.role = role.First();
            ViewBag.ProductID = new SelectList(inventoryService.GetInventories().Where(x=>x.Active==1), "ProductId", "DropdownStr");
            ViewBag.SaleID = new SelectList(iSaleService.GetSales(), "SaleId", "SaleId");

            foreach (SaleItem saleItem in allSaleItems)
            {
                saleItem.TotalSiPrice += saleItem.Quantity * (double)inventoryService.GetById(saleItem.ProductId).Quantity;
                saleItem.TotalSI += saleItem.Quantity;
                iSaleService.GetById(saleItem.SaleId).TotalSalePrice += saleItem.TotalSiPrice;
                iSaleService.GetById(saleItem.SaleId).TotalSaleItems+= saleItem.TotalSI;
            }
            return View(allSaleItems);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create(SaleItem saleItem)
        {
            if (ModelState.IsValid)
            {
                var SI = saleItem;
                var inv = inventoryService.GetInventories().Find(SI.ProductId); 
                SI.Inventory = inv;
                Sale s = iSaleService.GetSales().Find(HttpContext.Session.GetInt32("Current SaleID"));
                SI.Sale = s;
                SI.ProductId = saleItem.ProductId;
                SI.Returned = 0;
                SI.SaleId = s.SaleId;
                SI.TotalSI += SI.Quantity;
                if (SI.Inventory != null)
                {
                    inventoryService.GetById(SI.ProductId).RestQuantity -= SI.Quantity;
                    inventoryService.GetById(SI.ProductId).SaleQuantity += SI.Quantity;
                }
                if (inv != null)
                    SI.TotalSiPrice += SI.Quantity * (double)inv.SalePrice;
                await iSaleItem.CreateAsync(SI);
                return RedirectToAction("Create", new { id = s.SaleId.ToString() });
            }

            ViewBag.ProductID = new SelectList(inventoryService.GetInventories(), "ProductId", "DropdownStr", saleItem.ProductId);
            ViewBag.SaleID = new SelectList(iSaleService.GetSales(), "SaleId", "SaleId", saleItem.SaleId);
            return View(saleItem);
        }
        // Return a specific item given a SaleItem ID
        public ActionResult Return(int? id)
        {
            var session = HttpContext.Session;
            session.SetInt32("SOReturnItem", (int)id);
            session.SetString("SOReturnError", "");
            return RedirectToAction("ReturnItem");
        }

        // Contains text box to enter desired Quantity
        public ActionResult ReturnItem()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ProcessAdd()
        {
            var session = HttpContext.Session;
            bool xisvalid = true;
            int quantityToReturn = 0;
            try
            {
                quantityToReturn = Int32.Parse(HttpContext.Request.Form["qty"].ToString());
            }
            catch
            {
                xisvalid = false;
            }
            SaleItem si = iSaleItem.GetSaleItem().Find(session.GetInt32("SOReturnItem"));
            if (si == null)
            {
                xisvalid = false;
            }
            else
            {
                int q = si.Quantity;
                if ((quantityToReturn < 1) || (quantityToReturn > q))
                {
                    xisvalid = false;
                }
            }
            if (xisvalid)
            {
                si.Quantity += quantityToReturn;
                inventoryService.GetById(si.ProductId).RestQuantity -= quantityToReturn;
                inventoryService.GetById(si.ProductId).SaleQuantity += quantityToReturn;
                si.TotalSI += si.Quantity;
                if (si.Quantity == 0)
                {
                    si.Returned = 1;
                }
                await iSaleItem.UpdateAsync(si);
                session.SetString("SOReturnError", "");
                return RedirectToAction("Details", "Sale", new { id = si.SaleId });
            }
            else
            {
                session.SetString("SOReturnError", "Dữ liệu có vấn đề");
                return RedirectToAction("ReturnItem");
            }
        }
        [HttpPost]
        public async Task<IActionResult> ProcessReturn()
        {
            var session = HttpContext.Session;
            bool xisvalid = true;
            int quantityToReturn = 0;
            try
            {
                quantityToReturn = Int32.Parse(HttpContext.Request.Form["qty"].ToString());
            }
            catch
            {
                xisvalid = false;
            }
            SaleItem si = iSaleItem.GetSaleItem().Find(session.GetInt32("SOReturnItem"));
            if (si == null)
            {
                xisvalid = false;
            }
            else
            {
                int q = si.Quantity;
                if ((quantityToReturn < 1) || (quantityToReturn > q))
                {
                    xisvalid = false;
                }
            }
            if (xisvalid)
            {
                si.Quantity -= quantityToReturn;
                inventoryService.GetById(si.ProductId).RestQuantity += quantityToReturn;
                inventoryService.GetById(si.ProductId).SaleQuantity -= quantityToReturn;
                si.TotalSI -= si.Quantity;
                if (si.Quantity == 0)
                {
                    si.Returned = 1;
                }
                await iSaleItem.UpdateAsync(si);
                session.SetString("SOReturnError", "");
                return RedirectToAction("Details", "Sale", new { id = si.SaleId });
            }
            else
            {
                session.SetString("SOReturnError", "Dữ liệu có vấn đề");
                return RedirectToAction("ReturnItem");
            }
        }
        public async Task<IActionResult> Cancel(int? id)
        {
            var x = iSaleItem.GetSaleItem().Find((int)id);
            inventoryService.GetById(x.ProductId).Quantity += x.Quantity;
            await iSaleItem.Delete(x);
            return RedirectToAction("Create", new { id = HttpContext.Session.GetInt32("Current SaleID") });
        }
    }
}

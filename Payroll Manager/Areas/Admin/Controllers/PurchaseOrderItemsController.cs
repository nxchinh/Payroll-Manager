using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Primitives;
using Payroll_Manager.Areas.Admin.Models.VM_PurchaseOrderItem;
using Payroll_Manager.Entity.ModelForUser;
using Payroll_Manager.Persistence;
using Payroll_Manager.Services;

namespace Payroll_Manager.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class PurchaseOrderItemsController : Controller
    {
        private readonly IPurchaseOrderItem ipOrderItem;
        private readonly IInventoryService inventoryService;
        private readonly IPurchaseOrderService ipPurchaseOrderService;
        private readonly ApplicationDbContext _context;
        public PurchaseOrderItemsController(IPurchaseOrderItem ipOrderItem, IInventoryService inventoryService, IPurchaseOrderService ipPurchaseOrderService, ApplicationDbContext context)
        {
            this.ipOrderItem = ipOrderItem;
            this.inventoryService = inventoryService;
            this.ipPurchaseOrderService = ipPurchaseOrderService;
            _context = context;
        }
        public IActionResult Index()
        {
            var purchaseOrderItems = ipOrderItem.GetAll();
            return View(purchaseOrderItems.ToList());
        }
        public double calcTotalCost(PurchaseOrderItem p)
        {
            return (double)p.Quantity * (double)p.Inventory.NetPrice;
        }
        public ActionResult Create()
        {
            ViewBag.ProductID = new SelectList(inventoryService.GetInventories().Where(x=>x.Active == 1), "ProductId", "DropdownStr");
            ViewBag.PurchaseOrderID = new SelectList(ipPurchaseOrderService.GetPurchaseOrders(), "PurchaseOrderId", "PurchaseOrderId");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PurchaseOrderItem purchaseOrderItem)
        {
            if (ModelState.IsValid)
            {
                PurchaseOrderItem ans = purchaseOrderItem;
                Inventory inv = inventoryService.GetInventories().Find(Int32.Parse(HttpContext.Request.Form["ProductId"]));
                var id = Int32.Parse(HttpContext.Session.GetString("currPo"));
                PurchaseOrder po = ipPurchaseOrderService.GetPurchaseOrders().Find(id);
                ans = new PurchaseOrderItem
                {
                    Inventory = inv, PurchaseOrder = po, Received = 0,Quantity = purchaseOrderItem.Quantity,ProductId = purchaseOrderItem.ProductId,PurchaseOrderId = po.PurchaseOrderId
                };
                await ipOrderItem.CreateAsync(ans);

                return RedirectToAction("Details", "PurchaseOrders", new { id = ans.PurchaseOrderId.ToString() });
            }

            ViewBag.ProductID = new SelectList(inventoryService.GetInventories(), "ProductId", "Name", purchaseOrderItem.ProductId);
            ViewBag.PurchaseOrderID = new SelectList(ipPurchaseOrderService.GetPurchaseOrders(), "PurchaseOrderId", "PurchaseOrderId", purchaseOrderItem.PurchaseOrderId);
            return View(purchaseOrderItem);
        }
        public async Task<IActionResult> Cancel(int id)
        {
            var x = ipOrderItem.GetPurchaseOrdersItem().Find(id);
            await ipOrderItem.Delete(x);
            return RedirectToAction("Details", "PurchaseOrders", new { id = HttpContext.Session.GetString("currPo") });
        }


    }
}

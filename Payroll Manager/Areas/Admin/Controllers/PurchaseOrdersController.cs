using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Payroll_Manager.Entity.ModelForUser;
using Payroll_Manager.Persistence;
using Newtonsoft.Json;
using Payroll_Manager.Areas.Admin.Models.VM_Employee;
using Payroll_Manager.Areas.Admin.Models.VM_PurchaseOrder;
using Payroll_Manager.Services;

namespace Payroll_Manager.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class PurchaseOrdersController : Controller
    {
        private readonly IPurchaseOrderService iPurchaseOrderService;
        private readonly IPurchaseOrderItem iPurchaseOrderItem;
        private readonly IInventoryService inventory;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        public PurchaseOrdersController(IPurchaseOrderService iPurchaseOrderService, IPurchaseOrderItem item,
            UserManager<ApplicationUser> userManager, IInventoryService inventories, ApplicationDbContext context)
        {
            this.iPurchaseOrderService = iPurchaseOrderService;
            this.iPurchaseOrderItem = item;
            _userManager = userManager;
            inventory = inventories;
            _context = context;
        }
        public class PoWithItems
        {
            public IEnumerable<PurchaseOrderItem> ItemList { get; private set; }
            public PurchaseOrder PurchaseOrder { get; private set; }

            public PoWithItems(PurchaseOrder purchaseOrder, IEnumerable<PurchaseOrderItem> y)
            {
                ItemList = y;
                PurchaseOrder = purchaseOrder;
            }
        }

        // Get a poWithItems object based on a given purchase order ID and db instance
        public PoWithItems GetOrderWithItems(int givenId)
        {
            var ansPo = iPurchaseOrderService.GetPurchaseOrders().FirstOrDefault(x => x.PurchaseOrderId == givenId);
            IEnumerable<PurchaseOrderItem> ansList = iPurchaseOrderItem.GetAll().Where(x=>x.PurchaseOrderId == givenId);
            var e = ansList.ToList();
            foreach (var each in e)
            {
                double currLineCost = each.Quantity * (double)each.Inventory.NetPrice;
                each.TotalPrice = currLineCost;
            }

            _context.SaveChanges();
            var ans = new PoWithItems(ansPo, e);
            PoTotalSet(ans);
            _context.SaveChanges();
            return ans;

        }


        public void PoTotalSet(PoWithItems x)
        {
            var allItems = x.ItemList;
            double ans = 0;
            int soluongban = 0;
            foreach (var line in allItems)
            {
                double lineTotal = line.Quantity * (double) line.Inventory.NetPrice;
                ans += lineTotal;
                soluongban += line.Quantity;
            }

            x.PurchaseOrder.TotalPrice = ans;
            x.PurchaseOrder.SLBan = soluongban;
            return;
        }

        public IActionResult Index(int? pageNumber)
        {
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) != null)
            {
                int pageSize = 7;
                var employees = iPurchaseOrderService.GetAll().Select(employee => new PurchaseOrderIndexViewModel()
                {
                    PurchaseOrderId = employee.PurchaseOrderId,
                    OrderDate = employee.OrderDate.Date.ToShortDateString(),
                    price = GetTotalPrice(employee.PurchaseOrderId),
                    status = GetStatus(employee.PurchaseOrderId)
                }).ToList();
                return View(PaginatedList<PurchaseOrderIndexViewModel>.Create(employees, pageNumber ?? 1, pageSize));
            }
            else
            {
                return RedirectToAction("LowPermission");
            }
        }


        // Landing page
        public ActionResult PurchaseOrderHome()
        {
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("LowPermission");
            }
        }
        public IActionResult Details(int? id)
        {
            PurchaseOrder purchaseOrder =iPurchaseOrderService.GetById((int)id);
            if (purchaseOrder == null)
            {
                return NotFound();
            }
            var x = GetOrderWithItems(purchaseOrder.PurchaseOrderId);
            var session = HttpContext.Session;
            session.SetString("currPo", x.PurchaseOrder.PurchaseOrderId.ToString());
            return View(x);
        }
        public IActionResult Create()
        {
            PurchaseOrderCreateViewModel p = new PurchaseOrderCreateViewModel();
            return Create(p);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PurchaseOrderCreateViewModel purchaseOrder)
        {
            var applicationUser = _userManager.GetUserAsync(User);
            string userEmail = applicationUser?.Result.Email;
            if (ModelState.IsValid)
            {
                PurchaseOrder cat = new PurchaseOrder
                {
                    MaGiaoDich = Guid.NewGuid().ToString(),
                    PurchaseOrderId = purchaseOrder.PurchaseOrderId,
                    OrderDate = DateTime.Now,
                    IsReceived = false,
                    CreatedBy = userEmail,
                    CreatedDate = DateTime.Now,
                };

                iPurchaseOrderService.CreateAsync(cat);

                return RedirectToAction("Details", new { id = cat.PurchaseOrderId });
            }

            return View(purchaseOrder);
        }
        public async Task<IActionResult> Receive(int? id)
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(User);
            string userEmail = applicationUser?.Email;
            if (id == null)
            {
                return NotFound();
            }
            PoWithItems entireOrder = GetOrderWithItems((int)id);
            if (entireOrder == null)
            {
                return NotFound();
            }
            foreach (var line in entireOrder.ItemList.ToList())
            {
                var purchase = iPurchaseOrderItem.GetPurchaseOrdersItem().FirstOrDefault(x => x.PoItemId == line.PoItemId);
                inventory.GetById(purchase.ProductId).RestQuantity -= purchase.Quantity;
                inventory.GetById(purchase.ProductId).SaleQuantity += purchase.Quantity;
                purchase.Received = 1;
                purchase.UpdatedBy = userEmail;
                purchase.UpdatedDate = DateTime.Now;

                await iPurchaseOrderItem.UpdateAsync(purchase);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id = entireOrder.PurchaseOrder.PurchaseOrderId });
        }

        public IActionResult Cancel(int? id)
        {
            if (id != null)
            {
                PoWithItems entireOrder = GetOrderWithItems((int)id);
                foreach (var item in entireOrder.ItemList)
                {
                    iPurchaseOrderItem.Delete(item);
                }
                iPurchaseOrderService.Delete(entireOrder.PurchaseOrder);
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // Returns total price of a purchase order
        // given a purchase order ID
        public double GetTotalPrice(int id)
        {
            PoWithItems x = GetOrderWithItems(id);
            return x.PurchaseOrder.TotalPrice;
        }

        // Returns a string status OPEN or CLOSED
        // depending on the state of a PO(id)
        public string GetStatus(int id)
        {
            PoWithItems x = GetOrderWithItems(id);
            if (!(x.ItemList.Any()))
            {
                return "OPEN";
            }
            if (x.ItemList.First().Received == 0)
            {
                return "OPEN";
            }
            return "CLOSED";
        }

        // Method to control search functionality
        public ActionResult Search(string option, string search)
        {
            var session = HttpContext.Session;
            session.SetString("searchdebug",search);
            List<PoWithItems> poListComplete = new List<PoWithItems>();
            List<PurchaseOrder> poList = iPurchaseOrderService.GetAll().ToList();
            foreach (var item in poList)
            {
                int currId = item.PurchaseOrderId;
                PoWithItems currItem = GetOrderWithItems(currId);
                poListComplete.Add(currItem);
            }
            if (option == "ID")
            {
                try
                {
                    return View(poListComplete.Where(x => x.PurchaseOrder.PurchaseOrderId == Int32.Parse(search)).ToList());
                }
                catch
                {
                    return View(new List<PoWithItems>());
                }
            }
            else if (option == "Date")
            {
                return View(poListComplete.Where(x => x.PurchaseOrder.DateStr.Equals(search) || search == null).ToList());
            }
            else if (option == "Status")
            {
                return View(poListComplete.Where(x => GetStatus(x.PurchaseOrder.PurchaseOrderId).Equals(search) || search == null).ToList());
            }
            else
            {
                return View(new List<PoWithItems>());
            }
        }

        public ActionResult LowPermission()
        {
            return View();
        }
    }
}

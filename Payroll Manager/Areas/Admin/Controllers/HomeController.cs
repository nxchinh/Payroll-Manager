using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Payroll_Manager.Areas.Admin.Models;
using Payroll_Manager.Entity.ModelForUser;
using Payroll_Manager.Models;
using Payroll_Manager.Persistence;
using Payroll_Manager.Services;

namespace Payroll_Manager.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly ISaleService iSaleService;
        private readonly ISaleItem iSaleItem;
        private readonly IInventoryService inventoryService;
        private readonly IPurchaseOrderService iPurchaseOrderService;
        private readonly IPurchaseOrderItem iPurchaseOrderItem;
        private readonly Kiemtra kiemtra;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, Kiemtra kiemtra, ISaleService iSaleService, ISaleItem iSaleItem, IInventoryService inventoryService, IPurchaseOrderService iPurchaseOrderService, IPurchaseOrderItem iPurchaseOrderItem)
        {
            _logger = logger;
            _context = context;
            this.kiemtra = kiemtra;
            this.iSaleService = iSaleService;
            this.iSaleItem = iSaleItem;
            this.inventoryService = inventoryService;
            this.iPurchaseOrderService = iPurchaseOrderService;
            this.iPurchaseOrderItem = iPurchaseOrderItem;
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
        public PoWithItems GetPOrderWithItems(int givenId)
        {
            var ansPo = iPurchaseOrderService.GetPurchaseOrders().FirstOrDefault(x => x.PurchaseOrderId == givenId);
            IEnumerable<PurchaseOrderItem> ansList = iPurchaseOrderItem.GetAll().Where(x => x.PurchaseOrderId == givenId);
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
                double lineTotal = line.Quantity * (double)line.Inventory.NetPrice;
                ans += lineTotal;
                soluongban += line.Quantity;
            }

            x.PurchaseOrder.TotalPrice = ans;
            x.PurchaseOrder.SLBan = soluongban;
            return;
        }
        public double GetTotalPrice(int id)
        {
            PoWithItems x = GetPOrderWithItems(id);
            return x.PurchaseOrder.TotalPrice;
        }
        public IActionResult List()
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
                }
                variable.S.items = GetTotalSaleItems(variable.S.SaleId);
                variable.S.price = GetTotalSalePrice(variable.S.SaleId);
            }
            List<PoWithItems> poListComplete = new List<PoWithItems>();
            List<PurchaseOrder> poList = iPurchaseOrderService.GetAll().ToList();
            foreach (var item in poList)
            {
                int currId = item.PurchaseOrderId;
                PoWithItems currItem = GetPOrderWithItems(currId);
                poListComplete.Add(currItem);
            }

            var tongtien = 0.0;
            foreach (var variable in poListComplete)
            {
                tongtien += variable.PurchaseOrder.TotalPrice;
            }
            List<ThongKeTheoThang> result = soListComplete
                .GroupBy(l => l.S.SaleDate.Month)
                .Select(cl => new ThongKeTheoThang
                {
                    Thang = cl.Select(x => x.S.SaleDate.Month).FirstOrDefault(),
                    Soluong = cl.Count(),
                    TongTien = cl.Sum(c => c.S.price),
                }).ToList();
            return PartialView("List", result);
        }
        public IActionResult Index()
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
                }
                variable.S.items = GetTotalSaleItems(variable.S.SaleId);
                variable.S.price = GetTotalSalePrice(variable.S.SaleId);
            }
            List<PoWithItems> poListComplete = new List<PoWithItems>();
            List<PurchaseOrder> poList = iPurchaseOrderService.GetAll().ToList();
            foreach (var item in poList)
            {
                int currId = item.PurchaseOrderId;
                PoWithItems currItem = GetPOrderWithItems(currId);
                poListComplete.Add(currItem);
            }

            var tongtien = 0.0;
            foreach (var variable in poListComplete)
            {
                tongtien += variable.PurchaseOrder.TotalPrice;
            }
            List<ThongKeTheoThang> result = soListComplete
                .GroupBy(l => l.S.SaleDate.Month)
                .Select(cl => new ThongKeTheoThang
                {
                    Thang = cl.Select(x => x.S.SaleDate.Month).FirstOrDefault(),
                    Soluong = cl.Count(),
                    TongTien = cl.Sum(c => c.S.price),
                }).ToList();
            var DateOfToday = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var NextDay = DateOfToday.AddDays(1);
            var listDonHang = soListComplete.Where(s => s.S.SaleDate.CompareTo(DateOfToday) >= 0 && s.S.SaleDate.CompareTo(NextDay) < 0).ToList();

            double totalMoney = 0;
            int toDay = 0;

            if (listDonHang.Any())
            {
                toDay = listDonHang.Sum(x => x.S.items);
                totalMoney = listDonHang.Sum(x => x.S.price);

            }
            var beginDate = kiemtra.GetFirstDayOfWeek(DateTime.Now);
            var endDate = beginDate.AddDays(6);
            int sevenDay = 0;
            var listsevenDay = soListComplete.Where(s => s.S.SaleDate.CompareTo(beginDate) >= 0 && s.S.SaleDate.CompareTo(endDate) <= 0).ToList();
            if (listsevenDay.Any())
                sevenDay = listsevenDay.Sum(x => x.S.items);

            beginDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month + 1, 1);
            endDate = endDate.AddDays(-1);
            int thisMonth = 0;
            double totalMoneyMonth = 0;

            var listthisMonth = soListComplete.Where(s => s.S.SaleDate.CompareTo(beginDate) >= 0 && s.S.SaleDate.CompareTo(endDate) <= 0).ToList();
            if (listthisMonth.Any())
            { 
                thisMonth = listthisMonth.Sum(x => x.S.items);
                totalMoneyMonth = listthisMonth.Sum(x => x.S.price);
            }
            double AverageMoney = totalMoneyMonth / DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            ViewBag.TongDoanhThuSale = $"{result.Sum(x => x.TongTien):#,##0.00}";
            ViewBag.TongDoanhThuPaid = $"{tongtien:#,##0.00}";
            ViewBag.TongDoanhThu = $"{result.Sum(x => x.TongTien) + tongtien:#,##0.00}";
            ViewBag.ThoiGian = DateTime.UtcNow.ToString("HH:mm:ss");
            ViewBag.TongDonHangOnline = ThongKeDonHang();
            ViewBag.TongDonHangOffline = ThongKeGiaoDich();
            ViewBag.TongDonHang = ThongKeDonHang() + ThongKeGiaoDich();
            ViewBag.ThanhVien = ThongKeThanhVien();
            ViewBag.KhachHang = ThongKeKhachHang();
            ViewBag.SanPhamBanDuoc = ThongKeSanPham();
            ViewBag.Today = toDay;
            ViewBag.Week = sevenDay;
            ViewBag.ThisMonth = thisMonth;
            ViewBag.TotalMoney = $"{totalMoney:#,##0.00}";
            ViewBag.TotalMoneyMonth = $"{totalMoney:#,##0.00}";
            ViewBag.AverageMoneyMonth = $"{AverageMoney:#,##0.00}";
            return View(result);
        }
        public double ThongKeKhachHang()
        {
            double slddh = _context.Customers.Count();
            return slddh;
        }
        public double ThongKeDonHang()
        {
            double slddh = _context.Sales.Count();
            return slddh;
        }
        public double ThongKeGiaoDich()
        {
            double slddh = _context.PurchaseOrders.Count();
            return slddh;
        }
        public double ThongKeThanhVien()
        {
            double sltv = _context.Employees.Count();
            return sltv;
        }
        public int ThongKeSanPham()
        {
            int sanpham = _context.Inventories.Sum(n => n.SaleQuantity);
            return sanpham;
        }

        public ActionResult<List<BarModel>> GetBarData()
        {
            Random rnd = new Random();
            var ArraySell = new int[] { 0, 0, 0, 0, 0, 0, 0 };
            var thu = new string[] { "Thứ 2", "Thứ 3", "Thứ 4", "Thứ 5", "Thứ 6", "Thứ 7", "Chủ nhật" };
            var beginDate = kiemtra.GetFirstDayOfWeek(DateTime.Today); //Hàm GetFirstDayOfWeek,lấy thứ 2 datetime      
            beginDate = new DateTime(beginDate.Year, beginDate.Month, beginDate.Day);
            var NextDay = beginDate.AddDays(1);
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
                }
                variable.S.items = GetTotalSaleItems(variable.S.SaleId);
                variable.S.price = GetTotalSalePrice(variable.S.SaleId);
            }
            List<BarModel> chart = new List<BarModel>();
            for (int i = 0; i < 7; i++)
            {

                var value = soListComplete.Where(s => s.S.SaleDate.CompareTo(beginDate) >= 0 && s.S.SaleDate.CompareTo(NextDay) < 0).ToList();
                if (value.Count > 0)
                {
                    ArraySell[i] = value.Sum(x=>x.S.items);
                }
                beginDate = beginDate.AddDays(1);
                NextDay = beginDate.AddDays(1);
                chart.Add(new BarModel()
                {
                    Count = ArraySell[i],
                    Month = thu[i]
                });
            }
            return chart;
        }
        public ActionResult<List<BarModel>> GetBarData2()
        {
            Random rnd = new Random();
            var thu = new string[] { "Thứ 2", "Thứ 3", "Thứ 4", "Thứ 5", "Thứ 6", "Thứ 7", "Chủ nhật" };
            var ArrayWeeklySell = new int[] { 0, 0, 0, 0, 0, 0, 0 };
            var beginDate = kiemtra.GetFirstDayOfWeek(DateTime.Today); //Hàm GetFirstDayOfWeek,lấy thứ 2 datetime      
            beginDate = new DateTime(beginDate.Year, beginDate.Month, beginDate.Day);
            var NextDay = beginDate.AddDays(1);
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
                }
                variable.S.items = GetTotalSaleItems(variable.S.SaleId);
                variable.S.price = GetTotalSalePrice(variable.S.SaleId);
            }
            List<BarModel> chart = new List<BarModel>();
            for (int i = 0; i < 7; i++)
            {
                var tuantruoc = beginDate.AddDays(-7);
                var value2 = soListComplete.Where(s =>s.S.SaleDate == tuantruoc).ToList();
                if (value2.Count > 0)
                {
                    ArrayWeeklySell[i] = value2.Sum(x => x.S.items);
                }
                beginDate = beginDate.AddDays(1);
                NextDay = beginDate.AddDays(1);
                chart.Add(new BarModel()
                {
                    Count = ArrayWeeklySell[i],
                    Month = thu[i]
                });
            }
            return chart;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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
    }
}

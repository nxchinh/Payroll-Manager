using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Threading.Tasks;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;
using MailKit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using MimeKit;
using Payroll_Manager.Models;
using Payroll_Manager.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Payroll_Manager.Areas.Admin.Controllers;
using Payroll_Manager.Areas.Admin.Models.VM_Inventory;
using Payroll_Manager.Entity.ModelForUser;
using Payroll_Manager.Helpers;
using Payroll_Manager.Hubs;
using Payroll_Manager.Services;
using Payroll_Manager.Utilities;

namespace Payroll_Manager.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IInventoryService _inventoryService;
        private readonly AuctionService _auctionService;
        private readonly IStringLocalizer<UserController> _localizer;
        public class Product
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Photo { get; set; }
        }
        public UserController(ApplicationDbContext context,IInventoryService inventoryService, AuctionService auctionService, IStringLocalizer<UserController> localizer)
        {
            _context = context;
            _inventoryService = inventoryService;
            _auctionService = auctionService;
            _localizer = localizer;
        }
        public Email mails { get; set; }
        [Route("")]
        [Route("chongiaodien.html")]
        public IActionResult ChooseLayout()
        {
            return View();
        }
        [Route("giaodien2.html")]
        public IActionResult Test()
        {
            return View();
        }
        [Route("giaodien1.html")]
        public IActionResult Index()
        {
            ViewBag.Addresses = _inventoryService.GetAllInventories();
            var model = new EmailSendViewModel()
            {
                employee = _context.Employees.ToList(),
                _mails = mails,
            };
            return View(model);
        }
        [HttpGet]
        public IActionResult AddEmail()
        {
            ViewBag.Addresses = _inventoryService.GetAllInventories();
            var model = new EmailSendViewModel()
            {
                employee = _context.Employees.ToList(),
                _mails = mails,
            };
            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddEmailtest(EmailSendViewModel e)
        {
            ViewBag.Addresses = _inventoryService.GetAllInventories();
            if (e.Quatity <= _inventoryService.GetById(e.InventoriesId).Quantity && e.Quatity != 0)
            {
                using (var smtp = new SmtpClient())
                {
                    smtp.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtp.PickupDirectoryLocation = @"D:\MyMail\ticket_order";
                    var a = _inventoryService.GetById(e.InventoriesId).FullAd;
                    var msg = new MailMessage
                    {
                        Body = e.Name + " " + e.Email + " " + e.Phone + " " + e.Quatity + " " + a,
                        Subject = "Order",
                        From = new MailAddress("khoimessi99@gmail.com")
                    };
                    msg.To.Add("17dh111108@st.huflit.edu.vn");
                    await smtp.SendMailAsync(msg);
                }

                string content =
                    $"Có phải bạn muốn mua{_inventoryService.GetById(e.InventoriesId).FullAdv} Với số lượng là {e.Quatity} vé";
                string image = _inventoryService.GetById(e.InventoriesId).Image;
                new MailHelper().SendMail(e.Email, "Tin nhắn từ Evennet", content, image);
                var info = System.Globalization.CultureInfo.GetCultureInfo("vi-VN");
                TempData["Message"] = MessageHelper.YourBidOffer + $" {e.Email} là " +
                                      String.Format(info, "{0:c} VNĐ", (_inventoryService.GetById(e.InventoriesId).SalePrice * e.Quatity).ToString("N0"));
                var product = new Product()
                {
                    Id = "p01",
                    Name = "True",
                    Photo = "True"
                };
                return new JsonResult(product);
            }
            else
            {
                var product = new Product()
                {
                    Id = "p02",
                    Name = "False",
                    Photo = "False"
                };
                return new JsonResult(product);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddEmail(EmailSendViewModel e)
        {
            ViewBag.Addresses = _inventoryService.GetAllInventories();
            if(e.Quatity < _inventoryService.GetById(e.InventoriesId).Quantity)
            {
                using (var smtp = new SmtpClient())
                {
                    smtp.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtp.PickupDirectoryLocation = @"D:\MyMail\ticket_order";
                    var a = _inventoryService.GetById(e.InventoriesId).FullAd;
                    var msg = new MailMessage
                    {
                        Body = e.Name + " " + e.Email + " " + e.Phone + " " + e.Quatity + " " + a,
                        Subject = "Order",
                        From = new MailAddress("khoimessi99@gmail.com")
                    };
                    msg.To.Add("17dh111108@st.huflit.edu.vn");
                    await smtp.SendMailAsync(msg);
                }

                string content = "Có phải bạn muốn mua" + _inventoryService.GetById(e.InventoriesId).FullAdv + e.Quatity;
                string image = _inventoryService.GetById(e.InventoriesId).Image;
                new MailHelper().SendMail(e.Email, "Tin nhắn từ Evennet", content, image);
                var info = System.Globalization.CultureInfo.GetCultureInfo("vi-VN");
                TempData["Message"] = MessageHelper.YourBidOffer + $" {e.Email} là " +
                                      String.Format(info, "{0:c} VNĐ", _inventoryService.GetById(e.InventoriesId).SalePrice.ToString("N0"));
                return new JsonResult(new { success = true, responseText = "Your message successfuly sent!" });
            }
            return new JsonResult(new { success = false, responseText = "The attached file is not supported." });
        }
        [HttpGet]
        public IActionResult RealTimeTask()
        {
            ViewBag.Addresses = _inventoryService.GetAllInventories();
            var model = new EmailSendViewModel()
            {
                employee = _context.Employees.ToList(),
                _mails = mails,
            };
            return PartialView(model);
        }
        [HttpPost]
        public async Task<IActionResult> RealTimeTask(EmailSendViewModel e)
        {
            ViewBag.Addresses = _inventoryService.GetAllInventories();
            var info = System.Globalization.CultureInfo.GetCultureInfo("vi-VN");
            TempData["Message"] = MessageHelper.YourBidOffer + $" {e.Email} là " +
                                  String.Format(info, "{0:c} VNĐ", _inventoryService.GetById(e.InventoriesId).SalePrice.ToString("N0"));
            Inventory product = _inventoryService.GetById(e.InventoriesId);
            product.LuotDat += 1;   
            await _inventoryService.UpdateAsync(product);
            ViewBag.SuccessMsg = "Thank you";
            return RedirectToAction("Index", "User");
        }
        [HttpGet]
        public IActionResult AddEmail2()
        {
            return PartialView();
        }
        [HttpPost]
        public async Task<IActionResult> AddEmail2(EmailSendViewModel e)
        {
            using (var smtp = new SmtpClient())
            {
                smtp.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                smtp.PickupDirectoryLocation = @"D:\MyMail\event_contact";
                var msg = new MailMessage
                {
                    Body = e.Name ,
                    Subject = e._mails.Subject,
                    From = new MailAddress(e._mails.From)
                };
                msg.To.Add("17dh111108@st.huflit.edu.vn");
                await smtp.SendMailAsync(msg);
            }

            new MailHelper().SendMail(e.Email, "Tin nhắn từ Evennet", "Cảm ơn bạn đã để lại thông tin liên hệ",null);
            return RedirectToAction("Index", "User");
        }
        [HttpGet]
        public IActionResult AddEmail3()
        {
            return PartialView();
        }
        [HttpPost]
        public async Task<IActionResult> AddEmail3(EmailSendViewModel e)
        {
            using (var smtp = new SmtpClient())
            {
                smtp.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                smtp.PickupDirectoryLocation = @"D:\MyMail\event_subcribe";
                var msg = new MailMessage
                {
                    Body ="Tài khoản "+ e._mails.From + " đã đồng ý nhận quảng cáo từ trang" ,
                    Subject = "Subcribe",
                    From = new MailAddress(e._mails.From)
                };
                msg.To.Add("17dh111108@st.huflit.edu.vn");
                await smtp.SendMailAsync(msg);
            }
            new MailHelper().SendMail(e.Email, "Tin nhắn từ Evennet", "Cảm ơn bạn đã nhận quảng cáo từ công ty",null);
            return RedirectToAction("Index", "User");
        }
        [HttpGet]
        [Route("ve/{url}")]
        public async Task<IActionResult> Detail(int id,string url)
        {
            Inventory product = _context.Inventories.SingleOrDefault(p => p.Name == url);

            if (product != null)
                return View(new InventoryDetailViewModel
                {
                    ProductId = product.ProductId,
                    LuotDat = product.LuotDat,
                    Name = product.Name,
                    Image = product.Image,
                    SalePrice = product.SalePrice
                });
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> Buy(int id, string url)
        {
            decimal price = await _auctionService.Buy(id);
            return RedirectToAction("Detail", new { id = id, url = url });
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }
    }
}
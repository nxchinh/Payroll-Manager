using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Payroll_Manager.Areas.Admin.Models.VM_Event;
using Payroll_Manager.Entity.ModelForUser;
using Payroll_Manager.Handler;
using Payroll_Manager.Helpers;
using Payroll_Manager.Persistence;

namespace Payroll_Manager.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]

    public class EventController : Controller
    {
        public class Product
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public int Price { get; set; }
        }
        private readonly ApplicationDbContext context;

        public EventController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {

            return View();
        }
        [HttpPost]
        public IActionResult Index(EventViewModel model)
        {
            if (model.Id == 0)
            {
                new EventService(context).Add(model.ToEntity());
            }
            else
            {
                new EventService(context).Update(model.Id, model.ToEntity());
            }
            var res = GetList();
            return res;
        }

        [HttpGet]
        public IActionResult GetList()
        {
            var a = context.Events.ToList();
            List<EventViewModel> models = a.ToModelList();
            ViewBag.item = models;
            return PartialView("~/Areas/Admin/Views/Event/GetList.cshtml",models);
        }
        [HttpGet]
        public async Task<IActionResult> GetEventActive()
        {
            var posts = await context.EventImages
                .Where(p => p.Event.Active)
                .ToListAsync();
            return PartialView("~/Areas/Admin/Views/Event/GetEventActive.cshtml", posts);
        }
        public IActionResult Delete(int id)
        {
            new EventService(context).Delete(id).ToModel();
            var res = GetList();
            return res;
        }
        public IActionResult Restore(int id)
        {
            new EventService(context).Restore(id).ToModel();
            var res = GetList();
            return res;
        }
        [HttpGet]
        public IActionResult EditRecode(int id)
        {
            var model = context.Events.FirstOrDefault(x => x.Id == id);
            var eventmodel = new EventViewModel()
            {
                masanpham = model.Id,
                name = model.TenSuKien,
                date = model.ThoiGianBatDau
            };
            return new JsonResult(eventmodel);
        }
    }
}

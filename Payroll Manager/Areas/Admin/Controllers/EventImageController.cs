using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Payroll_Manager.Areas.Admin.Models.VM_Event;
using Payroll_Manager.Areas.Admin.Models.VM_File;
using Payroll_Manager.Entity;
using Payroll_Manager.Persistence;

namespace Payroll_Manager.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class EventImageController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public EventImageController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var Event = await context.Events.ToListAsync();
            ViewBag.Event = Event;
            ViewBag.Message = TempData["Message"];
            var fileuploadViewModel = await LoadAllFiles();
            return View(fileuploadViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> UploadToFileSystem(List<IFormFile> files, string description ,int EventID)
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(User);
            var Event = await context.Events.ToListAsync();
            ViewBag.Event = Event;
            foreach (var file in files)
            {
                var webRootPath = _hostingEnvironment.WebRootPath;
                var basePath = Path.Combine(webRootPath + "\\File\\");
                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                var filePath = Path.Combine(basePath, file.FileName);
                var extension = Path.GetExtension(file.FileName);
                if (!System.IO.File.Exists(filePath))
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    var fileModel = new EventImage()
                    {
                        CreatedOn = DateTime.UtcNow,
                        FileType = file.ContentType,
                        Extension = extension,
                        Name = fileName,
                        FullName = fileName + extension,
                        Description = description,
                        FilePath = filePath,
                        EventId = EventID,
                        UploadedBy = applicationUser.Email
                    };
                    context.EventImages.Add(fileModel);
                    context.SaveChanges();
                }
            }
            TempData["Message"] = "Đã thêm hình ảnh cho sự kiện";
            return RedirectToAction("Index");
        }
        private async Task<FileUploadViewModel> LoadAllFiles()
        {
            var viewModel = new FileUploadViewModel
            {
                EventFile = await context.EventImages.ToListAsync()
            };
            return viewModel;
        }
        public async Task<IActionResult> DownloadFileFromFileSystem(int id)
        {

            var file = await context.EventImages.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (file == null) return null;
            var memory = new MemoryStream();
            using (var stream = new FileStream(file.FilePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, file.FileType, file.Name + file.Extension);
        }
        public async Task<IActionResult> DeleteFileFromFileSystem(int id)
        {

            var file = await context.EventImages.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (file == null) return null;
            if (System.IO.File.Exists(file.FilePath))
            {
                System.IO.File.Delete(file.FilePath);
            }
            context.EventImages.Remove(file);
            context.SaveChanges();
            TempData["Message"] = $"Removed {file.Name + file.Extension} successfully from File System.";
            return RedirectToAction("Index");
        }
    }
}

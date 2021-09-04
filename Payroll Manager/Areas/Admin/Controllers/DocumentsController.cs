using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Payroll_Manager.Areas.Admin.Models;
using Payroll_Manager.Areas.Admin.Models.VM_Document;
using Payroll_Manager.Entity;
using Payroll_Manager.Extensions;
using Payroll_Manager.Persistence;
using Payroll_Manager.Persistence.Repository.Abstraction;
using Payroll_Manager.Storage;
using Payroll_Manager.Utilities;

namespace Payroll_Manager.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class DocumentsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<DocumentsController> _logger;
        private readonly IDataAccess _dataAccess;
        private readonly IMapper _mapper;
        private readonly FileStorage _storage;

        public DocumentsController(
            IDataAccess dataAccess,
            UserManager<ApplicationUser> userManager,
            IMapper mapper,
            FileStorage storage,
            ILogger<DocumentsController> logger)
        {
            _dataAccess = dataAccess;
            _userManager = userManager;
            _mapper = mapper;
            _storage = storage;
            _logger = logger;
        }

        public async Task<IActionResult> List(int page = 1)
        {

            var result = _dataAccess.Familiarization.FindAll().ToList();
            var a = result.GroupBy(x => x.Employee)
                .Select(y => new { Name = y.Key, Count = y.Count() });
            var b = await GetCurrentUserAsync();
            var employeeQuantity = _dataAccess.Familiarization.FindAll().Count(x => x.Employee == b.FullName);
            var quantity = _dataAccess.Familiarization.FindAll().Count();
            ViewBag.employeeRating = ((double)employeeQuantity / quantity ) * 100;
            ListViewModel model = new ListViewModel
            {
                Documents = _dataAccess.Document.GetList()
            };
            var data = JsonConvert.SerializeObject(a);
            ViewBag.DataPoints = data;
            return View(model);
        }

        [Authorize(Roles = SD.Admin)]
        public IActionResult Create()
        {
            return View(new DocumentViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = SD.Admin)]
        public async Task<IActionResult> Create(DocumentViewModel document)
        {
            if (!ModelState.IsValid)
                return View(document);
            var a = await GetCurrentUserAsync();
            DocumentInfo info = _mapper.Map<DocumentInfo>(document);

            info.Employee = a.FullName;
            info.CreatedAt = DateTime.Now;

            _dataAccess.Document.Create(info);
            _dataAccess.Save();

            _logger.LogInformation($"Tài liệu {info}");

            _storage[info.Id].AddFormFiles(document.Files);

            return RedirectToAction(nameof(List));
        }

        [Authorize(Roles = SD.Admin)]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            DocumentInfo document = _dataAccess.Document.FirstOrDefault(id.Value);  

            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = SD.Admin)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            DocumentInfo document = _dataAccess.Document.FirstOrDefault(id);

            _dataAccess.Document.Delete(document);
            _storage[id].Clear();

            _dataAccess.Save();

            var employee = await GetCurrentUserAsync();

            _logger.LogInformation($"Xác nhận xóa {document}. Tài liệu đã xóa thành công [{employee.FullName}({employee.Email})]");

            return RedirectToAction(nameof(List));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            DocumentInfo document = _dataAccess.Document.FirstOrDefault(id.Value);

            if (document == null)
            {
                return NotFound();
            }

            var a = await GetCurrentUserAsync();

            bool isFamiliarized = document.Familiarizations.Any(f => f.Employee == a.FullName);
            var check = document.Familiarizations.FirstOrDefault();
            bool ketqua = check != null && a.FullName.Equals(check.Employee);
            if (check != null)
            {
                return View(model: new DetailsViewModel { Document = document,Check = ketqua, СurrentUserIsFamiliarized = true || document.SeeBefore < DateTime.Now });
            }
            return View(model: new DetailsViewModel { Document = document, Check = false, СurrentUserIsFamiliarized = isFamiliarized || document.SeeBefore < DateTime.Now });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Attachment(int docId, Guid attId)
        {
            DocumentStorage attachments = _storage[docId];

            FileItem attachment = attachments.GetFile(attId);

            if (attachment == null) return NotFound();

            _logger.LogInformation($"Yêu cầu tài liệu đính kèm [ID={docId}]: {attachment.FileName}");

            return File(attachment.Stream, attachment.MimeType, attachment.FileName);
        }

        [HttpPost]
        public async Task<IActionResult> Acquainted(int id)
        {
            DocumentInfo document = _dataAccess.Document.FirstOrDefault(id);
            var a = await GetCurrentUserAsync();
            Familiarization familiarization = new Familiarization
            {
                Employee = a.FullName,
                DateTime = DateTime.Now
            };

            document.Familiarizations.Add(familiarization);
            
            _dataAccess.Document.Update(document);

            _dataAccess.Save();

            _logger.LogInformation($"С {document} Người làm [{familiarization.Employee}] ");

            return RedirectToAction(nameof(List));
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }
    }
}

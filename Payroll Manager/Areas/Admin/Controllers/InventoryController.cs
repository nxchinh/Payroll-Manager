using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Payroll_Manager.Areas.Admin.Models;
using Payroll_Manager.Areas.Admin.Models.VM_Inventory;
using Payroll_Manager.Entity;
using Payroll_Manager.Entity.ModelForUser;
using Payroll_Manager.Helpers;
using Payroll_Manager.Hubs;
using Payroll_Manager.Persistence;
using Payroll_Manager.Services;

namespace Payroll_Manager.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class InventoryController : Controller
    {
        private readonly IInventoryService _inventoryService;
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _hostingEnvironmentServices;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHubContext<AuctionHub> _auctionHub;
        private readonly ApplicationDbContext _context;
        public static readonly string SearchResult = "SearchResult";
        private readonly ILogger<InventoryController> _logger;
        private readonly IGoogleSearchService _googleSearchService;
        private readonly IBingSearchService _bingSearchService;
        private readonly IMemoryCache _memoryCache;

        public InventoryController(IInventoryService service, IWebHostEnvironment hostingService, ICategoryService categoryService, UserManager<ApplicationUser> userManager, ApplicationDbContext context, IHubContext<AuctionHub> auctionHub, ILogger<InventoryController> logger, IGoogleSearchService googleSearchService, IBingSearchService bingSearchService, IMemoryCache memoryCache)
        {
            _inventoryService = service;
            _hostingEnvironmentServices = hostingService;
            _categoryService = categoryService;
            _userManager = userManager;
            _context = context;
            _auctionHub = auctionHub;
            _logger = logger;
            _googleSearchService = googleSearchService;
            _bingSearchService = bingSearchService;
            _memoryCache = memoryCache;
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
        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation(1, $"Getting Statistic");

            return View("Get");
        }

        [HttpGet]
        [Route("/{keywords}/{tagurl}")]
        public async Task<IActionResult> Get(string keywords, string tagurl)
        {
            _logger.LogInformation(1, $"Getting Statistic");
            var response = new List<SearchResult>();
            try
            {
                if (_memoryCache.TryGetValue(SearchResult, out List<SearchResult> cacheEntry))
                {
                    response = cacheEntry;
                }
                else
                {
                    response.Add(await _googleSearchService.Search(keywords, tagurl));
                    response.Add(await _bingSearchService.Search(keywords, tagurl));
                    var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(30));
                    _memoryCache.Set(SearchResult, response, cacheEntryOptions);
                }
            }
            catch (Exception ex)
            {
                BadRequest(ex.Message);
            }
            return Ok(response);
        }
        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public IActionResult Index()
        {
            IEnumerable<Inventory> adds = _inventoryService.GetAll();
            InventoryIndexViewModel vm = new InventoryIndexViewModel
            {
                Inventories = adds
            };
            return View(adds);
        }

        public IActionResult Products()
        {
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) == null)
            {
                return RedirectToAction("LowPermission");
            }
            return View();
        }
        public IActionResult ViewProducts(int? id)
        {
            IEnumerable<Inventory> adds = _inventoryService.GetAll();
            if(id == null)
            {
                InventoryIndexViewModel vb = new InventoryIndexViewModel
                {
                    Inventories = adds
                };
                return View(vb);
            }
            Inventory p = _inventoryService.GetSingle(x => x.ProductId == id.Value);
            if (p == null)
            {
                return NotFound();
            }
            InventoryIndexViewModel vm = new InventoryIndexViewModel
            {
                Inventories = adds,
                InvenDetail = p
            };
            return View(vm);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(_categoryService.GetCategories(), "CategoryId", "CategoryName");
            InventoryCreateViewModel vm = new InventoryCreateViewModel();
            return View(vm);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(InventoryCreateViewModel vm)
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(User);
            string userEmail = applicationUser?.Email;

            string uniqueFileName = null;
            if (ModelState.IsValid)
            {
                if (vm.Image != null)
                {
                    string uploadFolder = Path.Combine(_hostingEnvironmentServices.WebRootPath + "\\Product_images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + vm.Image.FileName;
                    string filePath = Path.Combine(uploadFolder, uniqueFileName);
                    vm.Image.CopyTo(new FileStream(filePath, FileMode.Create));
                }
                Inventory inventory = new Inventory()
                {
                    ProductId = vm.ProductId,
                    CategoryId = vm.CategoryId,
                    Name = vm.Name,
                    Description = vm.Description,
                    NetPrice = vm.NetPrice,
                    SalePrice = vm.SalePrice,
                    Quantity = vm.Quantity,
                    Image = uniqueFileName,
                    CreatedBy = userEmail,
                    CreatedDate = DateTime.Now,
                    Active = 1,
                    SaleQuantity = 0,
                    RestQuantity = vm.Quantity - vm.RestQuantity
                };
                await _auctionHub.Clients.All.SendAsync(inventory.ProductId.ToString(), inventory.SalePrice);
                await _inventoryService.CreateAsync(inventory);
                return RedirectToAction("Products");
            }
            ViewBag.CategoryID = new SelectList(_categoryService.GetCategories(), "CategoryId", "CategoryName", vm.CategoryId);
            return RedirectToAction("Details", "Categories", new { id = vm.CategoryId });
        }
        public IActionResult Detail(int id)
        {
            Inventory p = _inventoryService.GetSingle(x => x.ProductId == id);
            return new JsonResult(p);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Update(int id)
        {
            Inventory inventory = _inventoryService.GetSingle(p => p.ProductId == id);

            InventoryCreateViewModel vm = new InventoryCreateViewModel
            {
                ProductId = inventory.ProductId,
                CategoryId = inventory.CategoryId,
                Name = inventory.Name,
                Description = inventory.Description,
                NetPrice = inventory.NetPrice,
                SalePrice = inventory.SalePrice,
                Quantity = inventory.Quantity,
                ImageURL = inventory.Image,
                Active = inventory.Active,
                SaleQuantity = inventory.SaleQuantity,
                RestQuantity = inventory.Quantity - inventory.SaleQuantity
            };
            ViewBag.CategoryID = new SelectList(_categoryService.GetCategories(), "CategoryId", "CategoryName", inventory.CategoryId);
            return View(vm);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(InventoryCreateViewModel vm)
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(User);
            string userEmail = applicationUser?.Email;
            string uniqueFileName = null;
            if (ModelState.IsValid)
            {
                if (vm.Image != null)
                {
                    string uploadFolder = Path.Combine(_hostingEnvironmentServices.WebRootPath + "\\images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + vm.Image.FileName;
                    string filePath = Path.Combine(uploadFolder, uniqueFileName);
                    vm.Image.CopyTo(new FileStream(filePath, FileMode.Create));
                    Inventory updatedInventory = new Inventory()
                    {
                        ProductId = vm.ProductId,
                        CategoryId = vm.CategoryId,
                        Name = vm.Name,
                        Description = vm.Description,
                        NetPrice = vm.NetPrice,
                        SalePrice = vm.SalePrice,
                        Quantity = vm.Quantity,
                        Image = uniqueFileName,
                        Active = 1,
                        SaleQuantity = vm.SaleQuantity,
                        RestQuantity = vm.Quantity - vm.SaleQuantity,
                        UpdatedBy = userEmail,
                        UpdatedDate = DateTime.Now
                    };
                    await _inventoryService.UpdateAsync(updatedInventory);
                }
                else
                {
                    Inventory updatedInventory = new Inventory()
                    {
                        ProductId = vm.ProductId,
                        CategoryId = vm.CategoryId,
                        Name = vm.Name,
                        Description = vm.Description,
                        NetPrice = vm.NetPrice,
                        SalePrice = vm.SalePrice,
                        Quantity = vm.Quantity,
                        Image = vm.ImageURL,
                        Active = 1,
                        SaleQuantity = vm.SaleQuantity,
                        RestQuantity = vm.Quantity - vm.SaleQuantity,
                        UpdatedBy = userEmail,
                        UpdatedDate = DateTime.Now
                    };
                    await _inventoryService.UpdateAsync(updatedInventory);
                }

            }
            ViewBag.CategoryID = new SelectList(_categoryService.GetCategories(), "CategoryId", "CategoryName", vm.CategoryId);
            return RedirectToAction("Details", "Categories", new { id = vm.CategoryId });
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            Inventory deletedinventory = _inventoryService.GetSingle(c => c.ProductId == id);
            deletedinventory.Active = 0;
            await _inventoryService.UpdateAsync(deletedinventory);
            return RedirectToAction("Details", "Categories", new { id = deletedinventory.CategoryId });
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Restore(int id)
        {
            Inventory restoredinventory = _inventoryService.GetSingle(c => c.ProductId == id);
            restoredinventory.Active = 1;
            await _inventoryService.UpdateAsync(restoredinventory);
            return RedirectToAction("Details", "Categories", new { id = restoredinventory.CategoryId });
        }
        public ActionResult LowPermission()
        {
            return View();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using AutoMapper;
using Microsoft.ApplicationInsights.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Payroll_Manager.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Payroll_Manager.Areas.Admin;
using Payroll_Manager.Extensions;
using Payroll_Manager.Hubs;
using Payroll_Manager.Services;
using Payroll_Manager.Services.Implementation;

namespace Payroll_Manager
{

    public class Startup
    {
        private IWebHostEnvironment iHostEnvironment;
        private readonly ILoggerFactory _loggerFactory;
        public Startup(IConfiguration configuration, IWebHostEnvironment iHostEnvironment)
        {
            Configuration = configuration;
            this.iHostEnvironment = iHostEnvironment;
        }

        public IConfiguration Configuration { get; }
        public static IConfiguration StaticConfig { get; private set; } // For use by static classes
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddMvc().AddControllersAsServices();
            //services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //    .AddEntityFrameworkStores<ApplicationDbContext>();

            //services.AddIdentity<ApplicationUser, IdentityRole>(config =>
            //{
            //    config.SignIn.RequireConfirmedEmail = true; 
            //}).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();


            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("vi-VN")
                };

                options.DefaultRequestCulture = new RequestCulture("en-US");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });
            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation();
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddDistributedMemoryCache();        
            services.AddSession(cfg => {                  
                cfg.Cookie.Name = "Payroll Manager";       
                cfg.IdleTimeout = new TimeSpan(0, 60, 0); 
            });
            services.Configure<IdentityOptions>(options =>
            {
                //Default Password Settings
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
                //Default Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
            });
            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                options.LoginPath = "/Identity/Account/Login";
                options.LogoutPath = "/Identity/Account/Logout";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConfiguration(Configuration.GetSection("Logging"));
                loggingBuilder.AddConsole();
                loggingBuilder.AddDebug();
            });
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.Strict;
                options.HttpOnly = HttpOnlyPolicy.None;
                options.Secure = iHostEnvironment.IsDevelopment()
                    ? CookieSecurePolicy.None : CookieSecurePolicy.Always;
            });
            services.AddControllersWithViews();
            services.AddMvc().AddControllersAsServices();
            services.AddRazorPages();
            services.AddScoped<UnitOfWork>();
            services.AddScoped<AuctionService>();
            services.AddScoped<Kiemtra>();
            services.AddSingleton<IJavaScriptSnippet, JavaScriptSnippet>();
            services.AddDatabaseAccess();
            // Add 'JavaScriptSnippet' "Service" for backwards compatibility. To remove in favour of 'IJavaScriptSnippet'. 
            services.AddSingleton<JavaScriptSnippet>();
            services.AddAutoMapper(GetType().Assembly);
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<IPayComputationService, PayComputationService>();
            services.AddScoped<INationalInsuranceContributionService, NationalInsuranceContributionService>();
            services.AddScoped<ITaxService, TaxService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IInventoryService, InventoryService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IPurchaseOrderItem, PurchaseOrderItemService>();
            services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();
            services.AddScoped<ISaleItem, SaleItemService>();
            services.AddScoped<ISaleService, SaleService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSignalR();
            services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
            {
                builder
                    .AllowAnyMethod()
                    .AllowAnyHeader() 
                    .AllowAnyOrigin() 
                    .AllowCredentials();
            }));
            services.AddFileStorage("Attachments");
            //services.AddSingleton<ICacheProvider, CacheProvider>(_ => new CacheProvider(_loggerFactory));
            int cacheSize = int.Parse("100");
            //services.AddSingleton(new RotatingCache<List<String>>(cacheSize));
            services.AddResponseCaching();
            services.AddHttpClient<IGoogleSearchService, GoogleSearchService>();
            services.AddHttpClient<IBingSearchService, BingSearchService>();
            services.AddSingleton<MemoryCache>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, 
                              IWebHostEnvironment env, 
                              UserManager<ApplicationUser> userManager, 
                              RoleManager<IdentityRole> roleManager,
                              ApplicationDbContext context)
        { 
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();

            var supportedCultures = new[]
            {
                new CultureInfo("en-US"),
                new CultureInfo("vi-VN"),
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-US"),
                // Formatting numbers, dates, etc.
                SupportedCultures = supportedCultures,
                // UI strings that we have localized.
                SupportedUICultures = supportedCultures
            });
            app.UseRequestLocalization();
            app.UseStaticFiles();
            app.UseSession();
            app.UseCookiePolicy();
            app.UseResponseCaching();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            DataSeedingIntializer.UserAndRoleSeedAsync(userManager, roleManager).Wait();
            DbInitializer.Initialize(context);
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=User}/{action=ChooseLayout}/{id?}");
                endpoints.MapAreaControllerRoute(name: "areas", "areas", pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
                endpoints.MapHub<AuctionHub>("/auction");
                endpoints.MapHub<ChatHub>("/chatHub");
            });
        }
    }
}

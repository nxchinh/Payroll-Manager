using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using Payroll_Manager.Persistence;
using Payroll_Manager.Utilities;

namespace Payroll_Manager.Areas.Admin.Controllers
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Dữ liệu sai")]
        [StringLength(150)]
        [Display(Name = "UserName")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Dữ liệu sai")]
        [StringLength(20)]
        [Display(Name = "Tên")]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "Dữ liệu sai")]
        [StringLength(20)]
        [Display(Name = "Họ")]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "Dữ liệu sai")]
        [StringLength(150), DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Dữ liệu sai")]
        [StringLength(150), DataType(DataType.Password), Compare(nameof(Password))]
        [Display(Name = "Xác nhận mất khẩu")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Dữ liệu sai")]
        [StringLength(80), EmailAddress, DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
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
        #region RolesAddAction
        public async Task MyRolesAddedAction()
        {
            if (!await _roleManager.RoleExistsAsync(SD.Roles.Admin.ToString()))
            {
                await _roleManager.CreateAsync(new IdentityRole(SD.Roles.Admin.ToString()));
            }
            if (!await _roleManager.RoleExistsAsync(SD.Roles.Hr.ToString()))
            {
                await _roleManager.CreateAsync(new IdentityRole(SD.Roles.Hr.ToString()));
            }
            if (!await _roleManager.RoleExistsAsync(SD.Roles.Payroll.ToString()))
            {
                await _roleManager.CreateAsync(new IdentityRole(SD.Roles.Payroll.ToString()));
            }
            if (!await _roleManager.RoleExistsAsync(SD.Roles.DepartmentHead.ToString()))
            {
                await _roleManager.CreateAsync(new IdentityRole(SD.Roles.DepartmentHead.ToString()));
            }
            if (!await _roleManager.RoleExistsAsync(SD.Roles.Worker.ToString()))
            {
                await _roleManager.CreateAsync(new IdentityRole(SD.Roles.Worker.ToString()));
            }

        }
        #endregion

        [Authorize(Roles = "Admin")]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser()
                {
                    FirstName = model.Firstname,
                    LastName = model.Lastname,
                    UserName = model.Username,
                    Email = model.Email
                };

                IdentityResult result = await _userManager.CreateAsync(user, model.Password);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    return View(model);
                }
                await _userManager.AddToRoleAsync(user, SD.Roles.Worker.ToString());

                await _signInManager.SignInAsync(user, true);

                SentMailToRegistered(user);


                return RedirectToAction("Index", "Home");

            }



            return View();
        }

        private static void SentMailToRegistered(ApplicationUser user)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Hệ thống Evenet", "khoitedu99@gmail.com"));
            message.To.Add(new MailboxAddress(user.UserName, user.Email));
            message.Subject = "Xác nhận thông tin";
            message.Body = new TextPart("plain")
            {
                Text = "Bạn được đăng ký trong hệ thống tính lương nhân sự và tiền lương."
            };

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("khoitedu99@gmail.com", "Fizz1999");

                client.Send(message);
                client.Disconnect(true);
            }
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/Account/Login");
        }
    }
}
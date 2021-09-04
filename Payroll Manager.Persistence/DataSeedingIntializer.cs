using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Payroll_Manager.Persistence
{
    public class DataSeedingIntializer
    {
        public static async Task UserAndRoleSeedAsync(UserManager<ApplicationUser> userManager,
                                                RoleManager<IdentityRole> roleManager)
        {

            string[] roles = { "Admin", "Hr", "Payroll", "DepartmentHead", "Worker" };
            foreach (var role in roles)
            {
                var roleExist = await roleManager.RoleExistsAsync(role);
                if (!roleExist)
                {
                    IdentityResult result = await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
            // Create Admin User
            if (userManager.FindByEmailAsync("khoimessi99@gmail.com").Result == null)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = "khoimessi99@gmail.com",
                    Email = "khoimessi99@gmail.com",
                    FirstName = "a",
                    LastName = "b"
                };
                IdentityResult identityResult = userManager.CreateAsync(user, "Fizz1999").Result;
                if (identityResult.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }

            //Create Manager User
            if (userManager.FindByEmailAsync("Hr@gmail.com").Result == null)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = "manager@gmail.com",
                    Email = "manager@gmail.com",
                    FirstName = "a",
                    LastName = "b"

                };
                IdentityResult identityResult = userManager.CreateAsync(user, "Fizz1999").Result;
                if (identityResult.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Hr").Wait();
                }
            }

            //Create Staff User
            if (userManager.FindByEmailAsync("Payroll@gmail.com").Result == null)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = "staff@gmail.com",
                    Email = "staff@gmail.com",
                    FirstName = "a",
                    LastName = "b"
                };
                IdentityResult identityResult = userManager.CreateAsync(user, "Fizz1999").Result;
                if (identityResult.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Payroll").Wait();
                }
            }
            //Create No Role User
            if (userManager.FindByEmailAsync("DepartmentHead@gmail.com").Result == null)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = "newStaff@gmail.com",
                    Email = "newStaff@gmail.com",
                    FirstName = "a",
                    LastName = "b"
                };
                IdentityResult identityResult = userManager.CreateAsync(user, "Fizz1999").Result;
                if (identityResult.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "DepartmentHead").Wait();
                }
            }
            if (userManager.FindByEmailAsync("Worker@gmail.com").Result == null)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = "Worker@gmail.com",
                    Email = "Worker@gmail.com",
                    FirstName = "a",
                    LastName = "b"
                };
                IdentityResult identityResult = userManager.CreateAsync(user, "Fizz1999").Result;
                if (identityResult.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Worker").Wait();
                }
            }
        }
    }
}

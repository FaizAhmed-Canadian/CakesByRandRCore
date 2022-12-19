using CakesByRAndRCore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data;
using System.Reflection.Metadata;

namespace CakesByRAndRCore.Data
{
    public static class SampleData
    {

        public static async Task InitializeRolesAndUser(AppIdentityDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {

            string[] appRoles = new string[] { "Admin", "ContentManager", "Customer" };

            foreach (string role in appRoles)
            {
                var roleStore = new RoleStore<IdentityRole>(context);

                if (!context.Roles.Any(r => r.Name == role))
                {
                    roleStore.CreateAsync(new IdentityRole(role));
                }
            }

            var user = new AppUser
            {
                FirstName = "Faiz",
                LastName = "Ahmed",
                Email = "bdfaiz@gmail.com",
                NormalizedEmail = "BDFAIZ@GMAIL.COM",
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                PhoneNumber = "+111111111111",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D")
            };

            if (!context.Users.Any(u => u.UserName == user.UserName))
            {
                var password = new PasswordHasher<AppUser>();
                var hashed = password.HashPassword(user, "secret");
                user.PasswordHash = hashed;

                var userStore = new UserStore<AppUser>(context);
                var result = userStore.CreateAsync(user);
            }

            AssignRoles(userManager, user.Email, appRoles);

            context.SaveChangesAsync();


        }

        public static async Task<IdentityResult> AssignRoles(UserManager<AppUser> userManager, string email, string[] roles)
        {
            //UserManager<AppUser> _userManager = services.GetService<UserManager<ApplicationUser>>();

            AppUser user = await userManager.FindByEmailAsync(email);
            var result = await userManager.AddToRolesAsync(user, roles);

            return result;
        }





        //public static async Task SeedSuperAdminAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        //{
        //    //Seed Default User
        //    var defaultUser = new AppUser
        //    {
        //        UserName = "superadmin",
        //        Email = "superadmin@gmail.com",
        //        FirstName = "Super",
        //        LastName = "Admin",
        //        EmailConfirmed = true,
        //        PhoneNumberConfirmed = true
        //    };
        //    if (userManager.Users.All(u => u.Id != defaultUser.Id))
        //    {
        //        var user = await userManager.FindByEmailAsync(defaultUser.Email);
        //        if (user == null)
        //        {
        //            await userManager.CreateAsync(defaultUser, "Password123!.");
        //            await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
        //            await userManager.AddToRoleAsync(defaultUser, Roles.ContentManager.ToString());
        //            await userManager.AddToRoleAsync(defaultUser, Roles.Customer.ToString());

        //        }

        //    }
        //}

    }
}

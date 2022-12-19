using CakesByRAndRCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Runtime.ConstrainedExecution;
using System.Xml.Linq;
using static CakesByRAndRCore.Models.StudentModel;
using Newtonsoft.Json.Serialization;
using CakesByRAndRCore.Repositories;
using System;
using Microsoft.EntityFrameworkCore;

namespace CakesByRAndRCore.Controllers
{

    [Authorize]
    public class AdminAndSetupController : Controller
    {

        //UnitOfWork unitOfWork = new UnitOfWork();

        private UserManager<AppUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

       // private readonly SignInManager<AppUser> signInManager;
        //private IPasswordHasher<AppUser> passwordHasher;


        public AdminAndSetupController(UserManager<AppUser> usrMgr, RoleManager<IdentityRole> roleMgr, IPasswordHasher<AppUser> passwordHash)
        {
            userManager = usrMgr;
            roleManager = roleMgr;
            //passwordHasher = passwordHash;
        }



        #region Miscellaneous Admin Work

    
        public IActionResult AdminHome()
        {
            return View();
        }



        #endregion



        #region User Management

        [AllowAnonymous]
        public IActionResult CreateUser()
        {

            CreateUserViewModel model = new CreateUserViewModel();
            model.YesNoList = PopulateYesNo();

            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel appUser)
        {

            if (!string.IsNullOrEmpty(appUser.Id))
            {

                //if (ModelState.IsValid)
                //{
                //    AppUser user = await userManager.FindByIdAsync(appUser.Id);
                //    if (user != null)
                //    {

                //        if (!string.IsNullOrEmpty(appUser.FirstName))
                //            user.FirstName = appUser.FirstName;

                //        if (!string.IsNullOrEmpty(appUser.LastName))
                //            user.LastName = appUser.LastName;

                //        if (!string.IsNullOrEmpty(appUser.Email))
                //            user.Email = appUser.Email;
                //        //else
                //        //    ModelState.AddModelError("", "Enter correct email address!");

                //        if (!string.IsNullOrEmpty(appUser.Password))
                //            user.PasswordHash = passwordHasher.HashPassword(user, appUser.Password);
                //        //else
                //        //    ModelState.AddModelError("", "Password cannot be empty!");

                //        if (!string.IsNullOrEmpty(appUser.UserName) && !string.IsNullOrEmpty(appUser.Email) && !string.IsNullOrEmpty(appUser.Password))
                //        {
                //            IdentityResult result = await userManager.UpdateAsync(user);
                //            if (result.Succeeded)
                //                return RedirectToAction("Index");
                //            else
                //                Errors(result);
                //        }
                //    }
                //    else
                //        ModelState.AddModelError("", "User Not Found!");
                //}

                return View(appUser);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    AppUser newAppUser = new AppUser
                    {
                        FirstName = appUser.FirstName,
                        LastName = appUser.LastName,
                        UserName = appUser.UserName,
                        Email = appUser.Email,
                        PhoneNumber = appUser.PhoneNumber

                    };

                    if (appUser.YesNoId == 1)
                        newAppUser.IsActive = true;
                    else
                        newAppUser.IsActive = false;


                    IdentityResult result = await userManager.CreateAsync(newAppUser, appUser.Password);

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(newAppUser, "Customer");
                        return RedirectToAction("Index", "Home", new { area = "" });
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                }
                }
                return View("CreateUser", appUser);

            }



        }

        public async Task<IActionResult> ManageProfile()
        {

            AppUser user = await userManager.GetUserAsync(User);

            if (user != null)
            {
                UserProfileViewModel userProfileViewModel = new UserProfileViewModel
                {

                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.UserName,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email,
                    ProfilePicture = user.ProfilePicture
                };

                return View(userProfileViewModel);
            }
            else
                return RedirectToAction("Index", "Home", new { area = "" });


        }

        [HttpPost]
        public async Task<IActionResult> SaveProfile(UserProfileViewModel model)
        {

            if (ModelState.IsValid)
            {
                AppUser user = await userManager.GetUserAsync(User);

                if (user != null)
                {

                    if (!string.IsNullOrEmpty(model.UserName) && !string.IsNullOrEmpty(model.FirstName) && !string.IsNullOrEmpty(model.LastName) && !string.IsNullOrEmpty(model.PhoneNumber))
                    {
                        user.UserName = model.UserName;
                        user.FirstName = model.FirstName;
                        user.LastName = model.LastName;
                        user.PhoneNumber = model.PhoneNumber;
                        user.Email = model.Email;


                        if (Request.Form.Files.Count > 0)
                        {
                            IFormFile file = Request.Form.Files.FirstOrDefault();
                            using (var dataStream = new MemoryStream())
                            {
                                await file.CopyToAsync(dataStream);
                                user.ProfilePicture = dataStream.ToArray();
                            }
                            //await userManager.UpdateAsync(user);
                        }

                        IdentityResult result = await userManager.UpdateAsync(user);
                        if (result.Succeeded)
                            return RedirectToAction("ManageProfile");
                        else
                            Errors(result);
                    }
                }
                else
                    ModelState.AddModelError("", "User Not Found!");

            }

            return View("ManageProfile", model);

        }

        public IActionResult ManageAddress()
        {
            return View();
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePassword(ChangePasswordViewModel model)
        {

            if (ModelState.IsValid)
            {
                AppUser user = await userManager.GetUserAsync(User);

                if (user != null)
                {

                    IdentityResult result = await userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ChangePassword");

                    }
                    else
                        Errors(result);
                }
                else
                    ModelState.AddModelError("", "User Not Found!");

            }

            return View("ManageProfile", model);


        }

        [HttpPost]
        public async Task<IActionResult> UpdatePassword2(ChangePasswordViewModel model)
        {

            if (ModelState.IsValid)
            {
                AppUser user = await userManager.GetUserAsync(User);

                if (user != null)
                {
                    IdentityResult result = await userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

                    if (result.Succeeded)
                    {
                        return Json(new { success = true, Data = "Password was changed successfully." });
                  
                        //Result callResult = new Result();
                        //callResult.Success = true;
                        //callResult.Data = "Password was changed successfully.";

                        //return Json(callResult);

                    }
                    else
                        return Json(new { success = false, Data = result.Errors.ToList().FirstOrDefault() });
                }
                else
                    return Json(new { success = false, Data = "User does not exist." });
            }

            return Json(new { success = false, Data = "Model not valid." });

        }


        public async Task<IActionResult> ViewUsers()
        {
            var users = await userManager.Users.ToListAsync();
            var userRolesViewModel = new List<UserRolesViewModel>();
            foreach (AppUser user in users)
            {
                var thisViewModel = new UserRolesViewModel();
                thisViewModel.UserId = user.Id;
                thisViewModel.Email = user.Email;
                thisViewModel.FirstName = user.FirstName;
                thisViewModel.LastName = user.LastName;
                thisViewModel.Roles = await GetUserRoles(user);
                userRolesViewModel.Add(thisViewModel);
            }
            return View(userRolesViewModel);
        }
        private async Task<List<string>> GetUserRoles(AppUser user)
        {
            return new List<string>(await userManager.GetRolesAsync(user));
        }


        public async Task<IActionResult> ManageUserRoles(string userId)
        {
            ViewBag.userId = userId;
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }
            ViewBag.UserName = user.UserName;
            var model = new List<ManageUserRolesViewModel>();
            foreach (var role in roleManager.Roles)
            {
                var userRolesViewModel = new ManageUserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.Selected = true;
                }
                else
                {
                    userRolesViewModel.Selected = false;
                }
                model.Add(userRolesViewModel);
            }
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> SaveUserRoles(List<ManageUserRolesViewModel> model, string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("");
            }
            var roles = await userManager.GetRolesAsync(user);
            var result = await userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return View(model);
            }
            result = await userManager.AddToRolesAsync(user, model.Where(x => x.Selected).Select(y => y.RoleName));
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return View(model);
            }
            return RedirectToAction("ViewUsers");
        }

        #endregion


        #region Role Management


        public async Task<IActionResult> AddRole()
        {
            var roles = await roleManager.Roles.ToListAsync();
            return View(roles);
        }


        [HttpPost]
        public async Task<IActionResult> AddRole(string roleName)
        {
            if (roleName != null)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName.Trim()));
            }
            return RedirectToAction("AddRole");
        }



        #endregion



        #region private Methods

        private static List<SelectListItem> PopulateYesNo()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Yes", Value = "1" });
            items.Add(new SelectListItem { Text = "No", Value = "0" });
            return items;
        }

        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }

        #endregion


    }
}

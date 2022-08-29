using FinalAgain.Areas.manage.ViewModels;
using FinalAgain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalAgain.Areas.manage.Controllers
{
    [Area("manage")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager,
                                      SignInManager<AppUser> signInManager,
                                      RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        //public async Task<IActionResult> CreateRole()
        //{
        //    await _roleManager.CreateAsync(new IdentityRole("User"));
        //    return Ok();
        //}
        //public async Task<IActionResult> CreateAdmin()
        //{
        //    AppUser admin = new AppUser()
        //    {
        //        UserName = "Admin"
        //    };
        //    var result = await _userManager.CreateAsync(admin, "Admin123!");
        //    if (!result.Succeeded)
        //    {
        //        return Ok(result.Errors);
        //    }
        //    var resultV2 = await _userManager.AddToRoleAsync(admin, "Admin");
        //    if (resultV2.Succeeded)
        //    {
        //        return Ok(result.Errors);
        //    }
        //    return Ok();
        //}
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel user)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(user.Username, user.Password, false, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Dashboard");
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");

            }
            return View(user);
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login");
        }
    }
}

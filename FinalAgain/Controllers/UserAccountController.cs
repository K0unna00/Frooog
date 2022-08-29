using FinalAgain.Areas.manage.ViewModels;
using FinalAgain.Helpers;
using FinalAgain.Models;
using FinalAgain.Utils;
using FinalAgain.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace FinalAgain.Controllers
{
    //[AllowAnonymous]
    public class UserAccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IWebHostEnvironment _env;

        public UserAccountController(UserManager<AppUser> userManager,
                                      SignInManager<AppUser> signInManager,
                                        IWebHostEnvironment env)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _env = env;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    UserName = model.UserName,
                    Email = model.Email
                };
                if (model.UserPPFile == null)
                {
                    user.UserPP = "pp.png";
                }
                else
                {
                    if (model.UserPPFile.ContentType == "image/png" || model.UserPPFile.ContentType == "image/jpeg")
                    {
                        user.UserPP = FileManager.Save(_env.WebRootPath, "uploads/Users", model.UserPPFile);
                    }
                    else
                    {
                        ModelState.AddModelError("UserPPFile", "Input png or jpg/jpeg");
                        return View();
                    }
                }
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("index", "Home");
                    //var roleResult = await _userManager.AddToRoleAsync(user, "User");
                    //if (roleResult.Succeeded)
                    //{
                    //    return RedirectToAction("index", "Home");
                    //}
                    //foreach (var error in roleResult.Errors)
                    //{
                    //    ModelState.AddModelError("", error.Description);
                    //}
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }
            if (model.ConfirmPassword != model.Password)
            {
                ModelState.AddModelError("ConfirmPassword", "Password and ConfirmPassword doestn match");
                return View(model);
            }
            bool hasLetter = model.Password.Any(char.IsLetter);
            bool hasDigit = model.Password.Any(char.IsDigit);
            bool hasSymbol = model.Password.Any(char.IsSymbol);
            if(!hasLetter || !hasDigit || !hasSymbol)
            {
                ModelState.AddModelError("Password", "Password must have at least 1 Digit , 1 symbol ");
                return View(model);
            }
            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginUserViewModel user)
        {
            if (!ModelState.IsValid)
            {
                if (user.Password == null)
                {
                    ModelState.AddModelError("Password", "Password is Required!!");
                }
            }
            else
            {
                var result = await _signInManager.PasswordSignInAsync(user.UserName, user.Password, false, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
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
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email )
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest();

            var isExists = await _userManager.FindByEmailAsync(email);
            if (isExists == null)
                return NotFound();

            var token = await _userManager.GeneratePasswordResetTokenAsync(isExists);

            var link = Url.Action("ResetPassword", "UserAccount", new { isExists.Id, token }, protocol: HttpContext.Request.Scheme);
            var message = $"<a href='{link}'>Click here</a>";

            await EmailUtil.SendEmailAsync(email, "Reset Password", message);
            return RedirectToAction("Login");
        }
        public async Task<IActionResult> ResetPassword(string id, string token)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(token))
                return BadRequest();

            var isExists = await _userManager.FindByIdAsync(id);
             
            if (isExists == null)
                return NotFound();

            ResetPasswordViewModel resetPasswordVW = new ResetPasswordViewModel
            {
                Email = isExists.Email,
                Token = token
            };
            return View(resetPasswordVW);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(string id, ResetPasswordViewModel resetPasswordVW)
        {

            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(resetPasswordVW.Token))
                return BadRequest();
            if (string.IsNullOrEmpty(resetPasswordVW.NewPassword) ||  string.IsNullOrEmpty(resetPasswordVW.ConfirmPassword))
            {
                ModelState.AddModelError("", "New password and Confirm password is required");
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            var isExists = await _userManager.FindByIdAsync(id);
            if (isExists == null)
                return RedirectToAction("error", "dashboard");
            var result = await _userManager.ResetPasswordAsync(isExists, resetPasswordVW.Token, resetPasswordVW.NewPassword);
            if (result.Errors == null)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }

            return RedirectToAction("Login");
        }

    }
}

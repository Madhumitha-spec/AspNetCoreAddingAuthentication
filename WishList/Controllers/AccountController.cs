using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WishList.Models;
using Microsoft.AspNetCore.Identity;
using WishList.Models.AccountViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;
using Microsoft.Azure.KeyVault.Models;
using System;

namespace WishList.Controllers

{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this._signInManager= signInManager;
            this._userManager= userManager;
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()

        {
            return View("Register");
        }
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Register(RegisterViewModel registerViewModel)
        {
            ApplicationUser user = new ApplicationUser();
            user.UserName = registerViewModel.Email;
            user.Email=registerViewModel.Email;
            Task<IdentityResult> result = _userManager.CreateAsync(user, registerViewModel.Password);
            if (!result.Result.Succeeded)
            {
                foreach (var error in result.Result.Errors)
                {
                    ModelState.AddModelError("Password", error.Description);
                }
                return View("Register", registerViewModel);
            }
            if (!ModelState.IsValid)
            {
                return View("Register",registerViewModel);
            }
            return RedirectToAction("Index","Home");
            
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View("Login");
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel loginViewModel)
        {
            ApplicationUser applicationUser = new ApplicationUser();
            applicationUser.UserName = loginViewModel.Email;
            applicationUser.Email=loginViewModel.Email;
            Task<Microsoft.AspNetCore.Identity.SignInResult> result=_signInManager.PasswordSignInAsync(applicationUser.UserName,loginViewModel.Password, false, false);
            if (!result.Result.Succeeded)
            {
                ModelState.AddModelError(String.Empty, "Invalid login attempt.");
            }
            if (!ModelState.IsValid) 
            { 
                return View(loginViewModel);
            }
            return RedirectToAction("Index","Item");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Index","Home");
        }
    }
   
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElcomManage.Data;
using ElcomManage.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ElcomManage.Controllers
{
    public class AccountsController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private ElcomDb _context;

        public AccountsController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ElcomDb context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public IActionResult Login(string returnUrl = null)
        {
            if(User.Identity.IsAuthenticated)
            {
               return RedirectToAction("Index", "Home");
            }
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginModel LoginModel,string returnUrl=null)
        {
            
            if (ModelState.IsValid)
            {

                var user=_context.Users.Where(u => u.UserName == LoginModel.Username).SingleOrDefault(); 
                if(user==null)
                {
                    ModelState.AddModelError(string.Empty, "Nuk ekziston ky user!");
                    return View(LoginModel);
                }
               var result=await _signInManager.PasswordSignInAsync(user, LoginModel.Password, true, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }

                ModelState.AddModelError("", "Wrong Username or Password!");
                return View(LoginModel);
            }

            return View(LoginModel);
        }
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Register()
        {
            ViewData["Roles"] = new SelectList(_context.Roles.ToList(), "Name", "Name");
            return View();
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterModel RegisterModel)
        {
            if(ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    FirstName = RegisterModel.Name,
                    Email=RegisterModel.Name,
                    LastName = RegisterModel.Surname,
                    UserName = RegisterModel.Username,
                };

                var result =await _userManager.CreateAsync(user, RegisterModel.Password);
                
                if(result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, RegisterModel.Role);
                    await _signInManager.SignInAsync(user, true);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(string.Empty, error.Description);
                }
                ViewData["Roles"] = new SelectList(_context.Roles, "Name", "Name");
                return View(RegisterModel);
            }
            ViewData["Roles"] = new SelectList(_context.Roles, "Name", "Name");
            return View(RegisterModel);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            
                return RedirectToAction("Login","Accounts");
        }
    }
}
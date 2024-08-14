using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;
using Web.Models;

namespace Web.Controllers
{
    [AllowAnonymous]
    public class AccountController : BaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ICommonMethods _commonMethods;
        private readonly SignInManager<AppUser> _signInManager;
        public AccountController(IConfiguration config, UserManager<AppUser> userManager,
            ICommonMethods commonMethods, SignInManager<AppUser> signInManager)
            : base(config)
        {
            _userManager = userManager;
            _commonMethods = commonMethods;
            _signInManager = signInManager;
        }


        [HttpGet("Register")]
        public IActionResult Register()
        {
            return View(new RegisterViewModel
            {
                RegisterCode = _commonMethods.GenerateCode(0, 10)
            });
        }

        [HttpPost("Register")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel register)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    UserName = register.UserName,
                    Email = register.Email,
                    FirstName = register.FirstName,
                    LastName = register.LastName,
                    Address = register.Address,
                    PhoneNumber = register.PhoneNumber,
                    BirthDate = register.BirthDate,
                    RegisteredAt = DateTime.UtcNow,
                    RegisterCode = register.RegisterCode,
                    IsActive = true
                };
                var imageResult = _commonMethods.SaveImage(register.File, _commonMethods.GenerateCode());

                if (imageResult == null)
                {
                    Notify("مشکلی در ذخیره تصویر بوجود آمد", "خطا", Domain.Shared.NotificationType.error);
                    return RedirectToAction("Index", "ManageUsers");
                }

                user.ImageName = imageResult;

                string defaultPassword = $"User_{_commonMethods.GenerateCode()}";
                var result = await _userManager.CreateAsync(user, $"{defaultPassword}");
                if (result.Succeeded)
                {
                    // ToDo: Send generated password for user

                    // await _userManager.AddToRoleAsync(user, "User");

                    Notify($"ثبت نام با موفقیت انجام شده. کلمه عبور: {defaultPassword}", "ثبت‌‌ نام", Domain.Shared.NotificationType.success);
                    return RedirectToAction("Index", "ManageUsers");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View();
        }

        [HttpGet("Login")]
        public IActionResult Login(string returnUrl = null)
        {
            if (_signInManager.IsSignedIn(User))
                return RedirectToAction("Index", "Home");

            var model = new LoginViewModel
            {
                ReturnUrl = returnUrl,
            };

            ViewData["returnUrl"] = returnUrl;
            return View(model);
        }

        [HttpPost("Login")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            if (_signInManager.IsSignedIn(User))
                return RedirectToAction("Index", "Home");

            model.ReturnUrl = returnUrl;

            ViewData["returnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user == null)
                {
                    ModelState.AddModelError("", "نام کاربری یا رمزعبور اشتباه است");
                    return View(model);
                }
                if (!user.IsActive)
                {
                    ModelState.AddModelError("", "حساب کاربری شما غیرفعال شده است");
                    return View(model);
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, true);

                if (result.Succeeded)
                {
                    await _signInManager.SignInWithClaimsAsync(user, model.RememberMe, new List<Claim>
                    {
                        // ToDo: Add nessecary claims
                    });

                    if(!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return RedirectToAction(returnUrl);

                    return RedirectToAction("Index", "Home");
                }
            }

            return View(model);
        }

        [HttpPost("Logout")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
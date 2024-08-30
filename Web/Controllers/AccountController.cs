using Application.Interfaces;
using Domain.Entities;
using Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Web.Models;

namespace Web.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ICommonMethods _commonMethods;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IUserAccessor _userAccessor;
        private readonly IMailService _mailService;

        public AccountController(IConfiguration config, UserManager<AppUser> userManager,
            ICommonMethods commonMethods, SignInManager<AppUser> signInManager, 
            IUserAccessor userAccessor, IMailService mailService)
            : base(config)
        {
            _userManager = userManager;
            _commonMethods = commonMethods;
            _signInManager = signInManager;
            _userAccessor = userAccessor;
            _mailService = mailService;
        }


        [HttpGet("Register")]
        [Authorize(Roles = "Programmer, Admin, Manager")]
        public IActionResult Register()
        {
            return View(new RegisterViewModel
            {
                RegisterCode = _commonMethods.GenerateCode(0, 10)
            });
        }

        [HttpPost("Register")]
        [Authorize(Roles = "Programmer, Admin, Manager")]
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
                    #region Send Confirmation Email
                    var userToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var ConfirmationLink = Url.Action("ConfirmEmail", "Account",
                        new { token = userToken, email = user.Email }, Request.Scheme);

                    var emailBody = _commonMethods.EmailTeplateReader("ثبت‌ نام در سایت", "EmailActivation.html", ConfirmationLink, defaultPassword);

                    await _mailService.SendEmail(user.Email, "فعال‌سازی حساب کاربری", emailBody);
                    #endregion

                    await _userManager.AddToRoleAsync(user, "User");
                    Notify($"ثبت نام با موفقیت انجام شد. کلمه عبور: {defaultPassword}", "ثبت‌‌ نام", Domain.Shared.NotificationType.success);
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
        [AllowAnonymous]
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
        [AllowAnonymous]
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
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim("IsPasswordChanged", user.IsPasswordChanged.ToString()),
                    });

                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    if (result.IsLockedOut)
                    {
                        ViewData["ErrorMessage"] = "اکانت شما به دلیل پنج بار ورود ناموفق به مدت پنج دقیقه قفل شده است";
                        return View(model);
                    }

                    ModelState.AddModelError("", "نام کاربری یا رمزعبور اشتباه است");
                    return View(model);
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

        [HttpGet("ForgotPassword")]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            if (_signInManager.IsSignedIn(User))
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost("ForgotPassword")]
        [AutoValidateAntiforgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    Notify("ایمیل وارد شده معتبر نمی‌باشد", "خطا", NotificationType.error);
                    return RedirectToAction("ForgotPassword", "Account");
                }
                if (await _userManager.IsEmailConfirmedAsync(user))
                {
                    var resetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                    string defaultPassword = $"User_{_commonMethods.GenerateCode()}";
                    user.IsPasswordChanged = false;
                    var result = await _userManager.ResetPasswordAsync(user, resetPasswordToken, defaultPassword);
                    await _userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                       var emailBody = _commonMethods.EmailTeplateReader("بازیابی کلمه‌عبور", "ForgotPassword.html", null, defaultPassword);

                        await _mailService.SendEmail(user.Email, "باریابی کلمه‌عبور", emailBody);
                        Notify($"کلمه‌عبور شما با موفقیت تغییر یافت. کلمه عبور: {defaultPassword}", "ثبت‌‌ نام", Domain.Shared.NotificationType.success);
                        return RedirectToAction("Login");
                    }
                }
                ModelState.AddModelError("", "حساب کاربری شما غیر فعال است");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ResetPassword(string returnUrl = null)
        {
            var user = await _userManager.FindByIdAsync(_userAccessor.GetUserId());
            if (user != null && user.IsPasswordChanged == true)
                return RedirectToAction("Index", "Profile");

            var model = new ResetPasswordViewModel
            {
                ReturnUrl = returnUrl,
            };

            ViewData["returnUrl"] = returnUrl;
            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                model.ReturnUrl = returnUrl;

                ViewData["returnUrl"] =returnUrl;

                var user = await _userManager.FindByIdAsync(_userAccessor.GetUserId());
                if (user == null) return RedirectToAction(nameof(Login));

                var resetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                user.IsPasswordChanged = true;
                var result = await _userManager.ResetPasswordAsync(user, resetPasswordToken, model.NewPassword);
                await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    Notify("کلمه‌عبور شما با موفقیت تغییر یافت", "تغییر کلمه‌عبور");

                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                    
                    return RedirectToAction("Index", "Profile");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }

        [AutoValidateAntiforgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return NotFound();

            if (token == null)
                return NotFound();

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                await _userManager.UpdateSecurityStampAsync(user);
                Notify("حساب شما با موفقیت فعال شد", "فعال‌سازی حساب کاربری");
                return RedirectToAction(nameof(Login));
            }

            return NotFound();
        }
    }
}
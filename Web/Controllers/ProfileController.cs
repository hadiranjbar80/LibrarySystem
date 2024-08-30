using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.Controllers
{
    [Authorize(Roles = "Programmer, Admin, Manager, Booker, User")]
    public class ProfileController : BaseController
    {
        private readonly IUserAccessor _userAccessor;
        private readonly UserManager<AppUser> _userManager;
        private readonly ICommonMethods _commonMethods;

        public ProfileController(IConfiguration config, IUserAccessor userAccessor,
            UserManager<AppUser> userManager, ICommonMethods commonMethods) : base(config)
        {
            _userAccessor = userAccessor;
            _userManager = userManager;
            _commonMethods = commonMethods;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> EditProfile(EditUserProfileInfoViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(model.Id)) return NotFound();
                var user = await _userAccessor.GetCurrentUser(model.Id);
                if (user == null) return NotFound();

                user.BirthDate = model.BirthDate;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.UserName = model.UserName;
                user.Address = model.Address;

                if (model.File != null && model.File.Length > 0)
                {
                    _commonMethods.DeleteImage(user.ImageName);
                    user.ImageName = _commonMethods.SaveImage(model.File, _commonMethods.GenerateCode());
                }

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    Notify("اطلاعات با موفقیت ویرایش شد", "ویرایش اطلاعات", Domain.Shared.NotificationType.success);
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return RedirectToAction("Index");
        }
    }
}

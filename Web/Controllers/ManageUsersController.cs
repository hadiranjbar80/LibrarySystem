using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Models;

namespace Web.Controllers
{
    [Authorize(Roles = "Admin, Programmer, Manager")]
    public class ManageUsersController : BaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ICommonMethods _commonMethods;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ManageUsersController(IConfiguration config, UserManager<AppUser> userManager,
            ICommonMethods commonMethods, RoleManager<IdentityRole> roleManager)
            : base(config)
        {
            _userManager = userManager;
            _commonMethods = commonMethods;
            _roleManager = roleManager;
        }

       // [HttpGet]
        public async Task<IActionResult> Index(string q)
        {
            var users = await _userManager.Users.ToListAsync();
            var usersListViewModel = new List<UsersListViewModel>();
            foreach (var user in users)
            {
                usersListViewModel.Add(new UsersListViewModel
                {
                    Address = user.Address,
                    BirthDate = user.BirthDate.ToString("yyyy/MM/dd"),
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    Image = user.ImageName,
                    RegisterCode = user.RegisterCode,
                    Id = user.Id,
                    IsActive = user.IsActive
                });
            }

            if(q != null)
            {
                usersListViewModel = usersListViewModel
                    .Where(u=>u.RegisterCode == q || u.FirstName.Contains(q) || u.LastName.Contains(q)).ToList();
            }

            ViewBag.q = q;
            return View(usersListViewModel);
        }

        #region EditUser

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            return View(new EditUserViewModel
            {
                BirthDate = user.BirthDate,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Id = user.Id,
                Address = user.Address,
                Email = user.Email,
                UserName = user.UserName,
                IsActive = user.IsActive,
            });
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(model.Id)) return NotFound();
                var user = await _userManager.FindByIdAsync(model.Id);
                if (user == null) return NotFound();

                user.BirthDate = model.BirthDate;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.PhoneNumber = model.PhoneNumber;
                user.UserName = model.UserName;
                user.Address = model.Address;
                user.Email = model.Email;
                user.IsActive = model.IsActive;

                if (model.File != null && model.File.Length > 0)
                {
                    _commonMethods.DeleteImage(user.ImageName);
                    user.ImageName = _commonMethods.SaveImage(model.File, _commonMethods.GenerateCode());
                }

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded) return RedirectToAction("Index");

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        #endregion

        #region AddUserToRole

        [HttpGet]
        public async Task<IActionResult> AddUserToRole(string id)
        {
            if(string.IsNullOrEmpty(id)) return NotFound();

            var user= await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var roles = await _roleManager.Roles.AsTracking()
                .Select(r => r.Name).ToListAsync();
            var userRoles = await _userManager.GetRolesAsync(user);
            var validRoles = roles.Where(r=> !userRoles.Contains(r))
                .Select(r=>new UserRolesViewModel(r)).ToList();
            var model = new AddUserToRoleViewModel(id, validRoles);

            return PartialView(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> AddUserToRole(AddUserToRoleViewModel model)
        {
            if(model == null) return NotFound();
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null) return NotFound();
            var requestedRoles = model.UserRoles.Where(r=>r.IsSelected)
                .Select(u=>u.RoleName)
                .AsQueryable();
            var result = await _userManager.AddToRolesAsync(user, requestedRoles.ToList());

            if (result.Succeeded) return RedirectToAction("Index");

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        #endregion

        #region RemoveUserFromRole

        [HttpGet]
        public async Task<IActionResult> RemoveUserFromRole(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                Notify("کاربری یافت نشد", "خطا", Domain.Shared.NotificationType.error);
                return RedirectToAction("Index");
            }
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                Notify("کاربری یافت نشد", "خطا", Domain.Shared.NotificationType.error);
                return RedirectToAction("Index");
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var validRoles = userRoles.Select(r => new UserRolesViewModel(r)).ToList();

            var model = new AddUserToRoleViewModel(id, validRoles);

            return PartialView(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> RemoveUserFromRole(AddUserToRoleViewModel model)
        {
            if(model == null) return NotFound();
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null) return NotFound();
            var requestedRoles = model.UserRoles.Where(r=>r.IsSelected)
                .Select(u=>u.RoleName)
                .AsQueryable();

            var result = await _userManager.RemoveFromRolesAsync(user, requestedRoles.ToList());

            if (result.Succeeded) return RedirectToAction("Index");

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        #endregion
    }
}
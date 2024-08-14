using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Models;

namespace Web.Controllers
{
    [Authorize(Roles = "Programmer")]
    public class ManageRolesController : BaseController
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public ManageRolesController(IConfiguration config, RoleManager<IdentityRole> roleManager)
            : base(config)
        {
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _roleManager.Roles.ToListAsync());
        }

        [HttpGet]
        public IActionResult AddRole()
        {
            return PartialView();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> AddRole(AddRolesViewModel model)
        {
            if (model.Name == null)
            {
                Notify("عنوان نقش را وارد کنید", "خطا", Domain.Shared.NotificationType.error);
                return RedirectToAction("Index");
            }

            var role = new IdentityRole(model.Name);
            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
                return RedirectToAction("Index");

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> DeleteRole(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null) return NotFound();
            await _roleManager.DeleteAsync(role);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var role = await _roleManager.FindByIdAsync(id);

            if (role == null) return NotFound();

            var model = new AddRolesViewModel
            {
                Id = role.Id,
                Name = role.Name,
            };

            return PartialView(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> EditRole(string id, AddRolesViewModel model)
        {
            if (model.Name == null)
            {
                Notify("عنوان نقش را وارد کنید", "خطا", Domain.Shared.NotificationType.error);
                return RedirectToAction("Index");
            }

            var role = await _roleManager.FindByIdAsync(model.Id);
            if (role == null) return NotFound();
            role.Name = model.Name;
            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded) return RedirectToAction("Index");

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }
    }
}

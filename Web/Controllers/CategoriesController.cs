using Application.Category.Commands;
using Application.Category.Dtos;
using Application.Category.Queries;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class CategoriesController : BaseController
    {
        public CategoriesController(IConfiguration configuration)
            : base(configuration) { }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await Mediator.Send(new List.Query { }));
        }

        #region Create Category

        [HttpGet]
        public IActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(CategoryDto category)
        {
            if (ModelState.IsValid)
            {
                var result = await Mediator.Send(new Create.Command { Category = category });
                if (result.IsSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            Notify("نام دسته‌بندی را وارد کنید", "خطا", Domain.Shared.NotificationType.error);
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Category Deletion

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await Mediator.Send(new Details.Query { Id = id });
            return PartialView(result.Value);
        }

        [HttpPost, ActionName("Delete")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> DeleteConfirm(Guid id)
        {
            await Mediator.Send(new Delete.Command { Id = id });
            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}

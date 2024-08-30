using Application.Interfaces;
using Application.Lending.Commands;
using Application.Lending.Dtos;
using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class LendingController : BaseController
    {
        private readonly IUserAccessor _userAccessor;
        private readonly ICheckerMethods _checkerMethods;
        private readonly IBookAccessor _bookAccessor;

        public LendingController(IConfiguration config, IUserAccessor userAccessor,
            ICheckerMethods checkerMethods, IBookAccessor bookAccessor)
            : base(config)
        {
            _userAccessor = userAccessor;
            _checkerMethods = checkerMethods;
            _bookAccessor = bookAccessor;
        }

        public async Task<IActionResult> Index()
        {
            var lendings = await Mediator.Send(new Application.Lending.Queries.List.Query { });
            return View(lendings.Value);
        }

        #region Lend

        [HttpGet]
        public async Task<IActionResult> Lend()
        {
            var model = new LendViewModel
            {
                UsersList = await _userAccessor.GetAllUsers(),
                BooksList = await _bookAccessor.GetAllAvailableBooks(),
                LendedAt = DateTime.Now.ToString("yyyy/MM/dd"),
            };

            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Lend(LendViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!await _checkerMethods.CheckUserSubscription(model.SelectedUser))
                {
                    Notify("اشتراک کاربر تمام شده است", "خطا", Domain.Shared.NotificationType.error);
                    return RedirectToAction("Index");
                }

                if (await _checkerMethods.CheckIfTheBookIsTaken(model.SelectedBook))
                {
                    Notify("کتاب مورد نظر در دسترس نمی‌باشد", "خطا", Domain.Shared.NotificationType.error);
                    return RedirectToAction("Index");
                }

                var lendingDto = new AddLendingDto
                {
                    BookCode = model.SelectedBook,
                    LendedAt = DateTime.Parse(model.LendedAt),
                    RegisterCode = model.SelectedUser,
                    ReturnAt = model.ReturnAt,
                };

                var result = await Mediator.Send(new Create.Command { LendingDto = lendingDto });

                if (result.IsSuccess)
                {
                    Notify("اطلاعات با موفقیت ثبت شد", "امانت کتاب");
                    return RedirectToAction("Index");
                }

                Notify(result.Error, "امانت کتاب");
                return RedirectToAction("Index");
            }

            model.UsersList = await _userAccessor.GetAllUsers();
            var books = await _bookAccessor.GetAllAvailableBooks();
            model.BooksList = books;
            return View(model);
        }

        #endregion


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReturnLending(string id, bool isBeingReturned)
        {
            var result = await Mediator.Send(new Edit.Command { Id = Guid.Parse(id), IsBeingReturned = isBeingReturned });
            if(result.IsSuccess)
            {
                Notify("اطلاعات با موفقیت ثبت شد", "برگشت امانت");
                return RedirectToAction("Index");
            }

            Notify(result.Error, "خطا", Domain.Shared.NotificationType.error);
            return RedirectToAction("Index");
        }
    }
}
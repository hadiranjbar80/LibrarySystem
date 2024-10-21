using Application.Book.Commands;
using Application.Book.Dtos;
using Application.Category.Queries;
using Microsoft.AspNetCore.Mvc;
using Domain.Shared;
using System.Text.RegularExpressions;
using Application.Core;
using Application.Category.Dtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Web.Controllers
{
    [Authorize(Roles = "Admin, Programmer, Manager, Booker")]
    public class BooksController : BaseController
    {
        private readonly ICommonMethods _commonMethods;
        public BooksController(IConfiguration config, ICommonMethods commonMethods)
            : base(config)
        {
            _commonMethods = commonMethods;
        }

        public async Task<IActionResult> Index(string searchQuery = null)
        {
            var result = await Mediator.Send(new Application.Book.Queries.List.Query { SearchQuery = searchQuery});
            ViewBag.SearchQuery = searchQuery;
            return View(result.Value);
        }

        #region Add Book

        [HttpGet]
        public async Task<IActionResult> AddBook()
        {
            return View(new BookDto
            {
                Code = _commonMethods.GenerateCode(),
                Categories = await GetCategories()
            });
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> AddBook(BookDto book)
        {
            if (ModelState.IsValid)
            {

                var result = await Mediator.Send(new Create.Command { Book = book });
                if (!string.IsNullOrWhiteSpace(result.Error))
                {
                    Notify(result.Error, "���", Domain.Shared.NotificationType.error);
                    return RedirectToAction(nameof(Index));
                }
                return RedirectToAction(nameof(Index));
            }
            book.Categories = await GetCategories();
            return View(book);
        }

        #endregion

        #region Book Deletion

        [HttpGet]
        public async Task<IActionResult> DeleteBook(Guid id)
        {
            var result = await Mediator.Send(new Application.Book.Queries.Details.Query { Id = id });
            if (result.Error == null)
                return PartialView(result.Value);

            Notify(result.Error, "���", NotificationType.error);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("Delete")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> DeleteConfirm(Guid id)
        {
            await Mediator.Send(new Delete.Command { Id = id });
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Edit Book

        [HttpGet]
        public async Task<IActionResult> EditBook(Guid id)
        {
            var book = await Mediator.Send(new Application.Book.Queries.Details.Query { Id = id });
            if (book == null)
            {
                Notify(book.Error, "���", NotificationType.error);
                return RedirectToAction(nameof(Index));
            }
            book.Value.Categories = await GetCategories();
            return View(book.Value);
        }

        [HttpPost]
        public async Task<IActionResult> EditBook(Guid id, BookDto book)
        {
            if (ModelState.IsValid)
            {
                var result = await Mediator.Send(new Edit.Command { Book = book });
                if (result.IsSuccess)
                {
                    return RedirectToAction("Index");
                }

                Notify(result.Error, "خطا", NotificationType.error);
                return RedirectToAction("Index");
            }

            book.Categories = await GetCategories();
            return View(book);
        }

        #endregion

        #region Private methods

        private async Task<List<CategoryDto>> GetCategories()
        {
            var categories = await Mediator.Send(new List.Query { });
            return categories.Value;
        }

        #endregion
    }
}
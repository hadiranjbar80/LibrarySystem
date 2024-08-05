using Application.Book.Commands;
using Application.Book.Dtos;
using Application.Category.Queries;
using Microsoft.AspNetCore.Mvc;
using Domain.Shared;
using System.Text.RegularExpressions;
using Application.Core;
using Application.Category.Dtos;

namespace Web.Controllers
{
    public class BooksController : BaseController
    {
        public BooksController(IConfiguration config)
            : base(config) { }

        public async Task<IActionResult> Index()
        {
            var result = await Mediator.Send(new Application.Book.Queries.List.Query { });
            return View(result.Value);
        }

        #region Add Book

        [HttpGet]
        public async Task<IActionResult> AddBook()
        {
            var bookCode = Regex.Replace(Guid.NewGuid().ToString().Replace("-", ""), "[A-Za-z ]", "");
            return View(new BookDto
            {
                Code = bookCode.Substring(0, 5),
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
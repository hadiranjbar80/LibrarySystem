using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Web.ViewComponents
{
    public class ShowUserLendings : ViewComponent
    {
        private readonly IBookAccessor _bookAccessor;
        private readonly IUserAccessor _userAccessor;

        public ShowUserLendings(IBookAccessor bookAccessor, IUserAccessor userAccessor)
        {
            _userAccessor = userAccessor;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("/Views/ViewComponents/ShowUserLendings.cshtml",
                await _userAccessor.GetUserLendings(_userAccessor.GetUserId()));
        }
    }
}

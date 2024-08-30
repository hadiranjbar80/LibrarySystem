using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.ViewComponents
{
    public class EditUserProfile : ViewComponent
    {
        private readonly IUserAccessor _userAccessor;

        public EditUserProfile(IUserAccessor userAccessor)
        {
            _userAccessor = userAccessor;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userAccessor.GetCurrentUser(_userAccessor.GetUserId());

            var model = new EditUserProfileInfoViewModel
            {
                Address = user.Address,
                BirthDate = user.BirthDate,
                FirstName = user.FirstName,
                Id = user.Id,
                LastName = user.LastName,
                UserName = user.UserName,
            };

            return View("/Views/ViewComponents/EditUserProfile.cshtml", model);
        }

    }
}

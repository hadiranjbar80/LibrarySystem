using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.ViewComponents
{
    public class ShowUserInfo : ViewComponent
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserAccessor _userAccessor;

        public ShowUserInfo(UserManager<AppUser> userManager, IUserAccessor userAccessor)
        {
            _userManager = userManager;
            _userAccessor = userAccessor;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userAccessor.GetCurrentUserSubscriptionIncluded(_userAccessor.GetUserId());

            var model = new UserInfoViewModel
            {
                Address = user.Address,
                BirthDate = user.BirthDate.ToString("yyyy/MM/dd"),
                Email = user.Email,
                FullName = $"{user.FirstName} {user.LastName}",
                PhoneNumber = user.PhoneNumber,
                UserName = user.UserName,
                RegisterCode = user.RegisterCode,
                RegisteredAt = user.RegisteredAt.ToString("yyyy/MM/dd"),
                ImageName = user.ImageName,
                SubscriptionExpireDate = (user.Subscription != null ? user.Subscription.EndDate.ToString("yyyy/MM/dd") : null),
            };

            return View("/Views/ViewComponents/ShowUserInfo.cshtml" ,model);
        }
    }
}

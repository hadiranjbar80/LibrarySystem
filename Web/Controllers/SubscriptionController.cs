using Application.Interfaces;
using Application.Subscription.Commands;
using Application.Subscription.Dtos;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ZarinPal.Class;

namespace Web.Controllers
{
    public class SubscriptionController : BaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserAccessor _userAccessor;

        public SubscriptionController(IConfiguration configuration,
            UserManager<AppUser> userManager, IUserAccessor userAccessor)
            : base(configuration)
        {
            _userManager = userManager;
            _userAccessor = userAccessor;
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> AddSubscription()
        {
            var userSubscription = await _userAccessor.GetUserSubscription(_userAccessor.GetUserId());

            // Checking if the user has already a subscription record in the database;
            // if there's any record related to the current loged in user it'll just update the dates.
            if (userSubscription != null)
            {
                SubscriptionDto subscriptionDto = new SubscriptionDto
                {
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddMonths(12),
                    IsFinally = false,
                    Id = userSubscription.Id,
                };

                var res = await Mediator.Send(new Edit.Command { Subscription = subscriptionDto });
                if (res.IsSuccess)
                {
                    Notify("اشتراک شما با موفقیت تمدید شد", "خرید اشتراک", Domain.Shared.NotificationType.success);
                    return RedirectToAction("Index", "Profile");
                }
                Notify("مشکلی در خرید اشتراک پیش آمده است، لطفا چند دقیقه بعد امتحان کنید.", "خطا", Domain.Shared.NotificationType.error);
                return RedirectToAction("Index", "Profile");
            }

            var result = await Mediator.Send(new Create.Command { });
            if (!result.IsSuccess)
            {
                Notify("مشکلی در خرید اشتراک پیش آمده است، لطفا چند دقیقه بعد امتحان کنید.", "خطا", Domain.Shared.NotificationType.error);
                return RedirectToAction("Index", "Profile");
            }

            Notify("اشتراک شما با موفقیت تمدید شد", "خرید اشتراک", Domain.Shared.NotificationType.success);
            return RedirectToAction("Index", "Profile");
        }
    }
}

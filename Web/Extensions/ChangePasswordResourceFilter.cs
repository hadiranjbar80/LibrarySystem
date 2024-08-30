using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Web.Extensions
{
    public class ChangePasswordResourceFilter : IAsyncResourceFilter
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUrlHelperFactory _urlHelperFactory;

        public ChangePasswordResourceFilter(UserManager<AppUser> userManager
            ,IUrlHelperFactory urlHelperFactory)
        {
            _userManager = userManager;
            _urlHelperFactory = urlHelperFactory;
        }

        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            var urlHelper = _urlHelperFactory.GetUrlHelper(context);
            var redirectUrl = urlHelper.Action("/ManageUsers/PasswordChange");
            var currentUrl = context.HttpContext.Request.Path;

            if(redirectUrl != currentUrl)
            {
                var user = await _userManager.GetUserAsync(context.HttpContext.User);
                if(user != null && user.IsPasswordChanged == false && 
                    context.RouteData.Values["action"].ToString() != "ResetPassword")
                {
                    //context.Result = new RedirectResult(redirectUrl);
                    context.Result = new RedirectToActionResult("ResetPassword", "Account", new { ReturnUrl = context.HttpContext.Request.Path });
                    return;
                }
            }
            await next();
        }
    }
}

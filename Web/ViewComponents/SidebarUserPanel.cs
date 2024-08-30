using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.ViewComponents
{
    public class SidebarUserPanel : ViewComponent
    {
        private readonly IUserAccessor _userAccessor;

        public SidebarUserPanel(IUserAccessor userAccessor)
        {
            _userAccessor = userAccessor;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userAccessor.GetCurrentUser(_userAccessor.GetUserId());

            if (user == null) throw new Exception("Unauthorized");

            SidebarPanelViewModel model = new SidebarPanelViewModel
            {
                ImageName = user.ImageName,
                UserName = user.UserName,
            };

            return View("/Views/ViewComponents/SidebarUserPanel.cshtml" ,model);
        }
    }
}

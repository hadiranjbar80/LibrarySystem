namespace Web.Models
{
    public class UserInfoViewModel
    {
        public string FullName { get; set; }
        public string RegisterCode { get; set; }
        public string PhoneNumber { get; set; }
        public string RegisteredAt { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string BirthDate { get; set; }
        public string ImageName { get; set; }
        public string SubscriptionExpireDate { get; set; }
    }

    public class SidebarPanelViewModel
    {
        public string ImageName { get; set; }
        public string UserName { get; set; }
    }
}

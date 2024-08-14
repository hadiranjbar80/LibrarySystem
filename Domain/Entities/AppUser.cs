using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RegisterCode { get; set; }
        public string Address { get; set; }
        public string ImageName { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime RegisteredAt { get; set; }
        public bool IsPasswordChaned { get; set; } = false;
        public bool IsActive { get; set; } = false;
    }
}
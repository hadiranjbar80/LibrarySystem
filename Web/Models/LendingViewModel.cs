
using Application.Book.Dtos;
using Domain.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class LendViewModel
    {
        public List<Book> BooksList { get; set; }
        public List<AppUser> UsersList { get; set; }
        [DisplayName("تاریخ امانت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string LendedAt { get; set; }
        [DisplayName("تاریخ برگشت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public DateTime ReturnAt { get; set; }
        [DisplayName("کاربر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string SelectedUser { get; set; }
        [DisplayName("کتاب")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string SelectedBook { get; set; }
    }
}

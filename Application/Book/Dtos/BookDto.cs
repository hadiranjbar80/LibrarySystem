using Application.Category.Dtos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Application.Book.Dtos
{
    public class BookDto
    {
        public Guid Id { get; set; }
        [DisplayName("دسته‌بندی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public Guid CategoryId { get; set; }
        [DisplayName("نام کتاب")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Name { get; set; }
        [DisplayName("نویسنده")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Author { get; set; }
        [DisplayName("ناشر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Publisher { get; set; }
        [DisplayName("کد کتاب")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Code { get; set; }
        public ICollection<CategoryDto> Categories { get; set; }
        
    }
}
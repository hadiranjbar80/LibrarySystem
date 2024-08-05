using Application.Category.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Book.Dtos
{
    public class BookListDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public string Code { get; set; }
        public CategoryDto Category { get; set; }
    }
}

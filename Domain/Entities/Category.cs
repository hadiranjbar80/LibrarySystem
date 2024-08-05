using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Category
    {
        
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<Book> Books { get; set; }
    }
}

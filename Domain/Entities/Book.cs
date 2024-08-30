using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }

        // Navigation props
        public Category Category { get; set; }
        public ICollection<Lending> Lendings { get; set; }
    }
}

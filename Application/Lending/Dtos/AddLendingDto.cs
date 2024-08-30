using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Lending.Dtos
{
    public class AddLendingDto
    {
        public string RegisterCode { get; set; }
        public string BookCode { get; set; }
        public DateTime LendedAt { get; set; }
        public DateTime ReturnAt { get; set; }
    }
}

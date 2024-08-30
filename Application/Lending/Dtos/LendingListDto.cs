using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Lending.Dtos
{
    public class LendingListDto
    {
        public Guid Id { get; set; }
        public string BookName { get; set; }
        public string BookCode { get; set; }
        public string UserFirstName { get; set; }
        public string UserLasttName { get; set; }
        public DateTime LendedAt { get; set; }
        public DateTime ReturnAt { get; set; }
        public bool IsBeingReturned { get; set; }
    }
}

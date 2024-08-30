using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IMailService
    {
        Task SendEmail(string email, string subject,string message);
    }
}

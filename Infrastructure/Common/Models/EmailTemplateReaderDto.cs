using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common.Models
{
    public class EmailTemplateReaderDto
    {
        public string Link { get; set; }
        public string Title { get; set; }
        public string TemplateName { get; set; }
        public string Password { get; set; }
    }
}

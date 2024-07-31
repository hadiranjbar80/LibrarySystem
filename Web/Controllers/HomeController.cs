using Application.Category.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, 
            IConfiguration configuration)
            : base(configuration)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}

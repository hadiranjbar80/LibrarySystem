using Application.Core;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Web.Controllers
{
    public class BaseController : Controller
    {
        private IMediator _mediator;
        private readonly IConfiguration _config;

        protected IMediator Mediator => _mediator ??=
            HttpContext.RequestServices.GetService<IMediator>();

        public BaseController(IConfiguration config)
        {
            _config = config;
        }

        public void Notify(string message, string title = "Sweet alert demo", 
            NotificationType type = NotificationType.success) 
        {
            var msg = new
            {
                message = message,
                title = title,
                type = type.ToString(),
                provider = _config["NotificationProvider"]
            };

            TempData["Message"] = JsonConvert.SerializeObject(msg);
        }

        private string GetProvider()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            IConfigurationRoot configuration = builder.Build();

            var value = configuration["NotificationProvider"];

            return value;
        }
    }
}

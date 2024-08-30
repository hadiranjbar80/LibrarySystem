using Application.Category.Commands;
using Application.Category.Queries;
using Application.Core;
using Application.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.Common;
using Infrastructure.Common.Models;
using Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Globalization;

namespace Web.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<ChangePasswordResourceFilter>();
            services.AddControllersWithViews(opt =>
            {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                opt.Filters.Add(new AuthorizeFilter(policy));
                opt.Filters.Add(typeof(ChangePasswordResourceFilter));
            });
            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });
            services.AddMediatR(typeof(List.Handler));
            services.AddAutoMapper(typeof(MappingProfiles).Assembly);
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<Create>();
            services.AddScoped<ICommonMethods, CommonMethods>();
            services.AddScoped<IUserAccessor, UserAccessor>();
            services.AddScoped<ICheckerMethods, CheckerMethods>();
            services.AddScoped<IBookAccessor, BookAccessor>();
            services.AddScoped<IMailService, MailService>();
            services.Configure<EmailSetting>(config.GetSection("EmailSetting"));


            CultureInfo.DefaultThreadCurrentCulture
              = CultureInfo.DefaultThreadCurrentUICulture
              = PersianDateExtensionMethods.GetPersianCulture();


            return services;
        }
    }
}

using Domain.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Persistence;

namespace Web.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddIdentity<AppUser, IdentityRole>(opt=>{
                opt.Password.RequiredUniqueChars = 0;
                opt.User.AllowedUserNameCharacters = 
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-";
                opt.Password.RequireNonAlphanumeric = false;
                opt.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<DataContext>()
            .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(opt=> {
                opt.AccessDeniedPath = "/Account/AccessDenied";
                opt.LoginPath = "/Login";
                opt.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
            });

            return services;
        }
    }
}
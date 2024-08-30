using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Security.Claims;

namespace Infrastructure.Security
{
    public class UserAccessor : IUserAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _context;
        private readonly UserManager<AppUser> _userManager;

        public UserAccessor(IHttpContextAccessor httpContextAccessor, DataContext context, UserManager<AppUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _userManager = userManager;
        }

        public async Task<List<AppUser>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<AppUser> GetCurrentUser(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<AppUser> GetCurrentUserSubscriptionIncluded(string id)
        {
            return await _context.Users.Include(x => x.Subscription).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<AppUser> GetUserByRegisterCode(string registerCode)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.RegisterCode == registerCode);
        }

        public string GetUserId()
        {
            return _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public async Task<List<Lending>> GetUserLendings(string userId)
        {
            return await _context.Lendings.Include(x => x.Book)
                .Where(x => x.AppUser.Id == userId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Subscription> GetUserSubscription(string id)
        {
            return await _context.Subscriptions.FirstOrDefaultAsync(X => X.UserId == id);
        }
    }
}

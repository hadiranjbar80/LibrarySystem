using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Infrastructure.Security
{
    public class CheckerMethods : ICheckerMethods
    {
        private readonly DataContext _context;

        public CheckerMethods(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> CheckIfTheBookIsTaken(string bookCode)
        {
            return await _context.Lendings.AnyAsync
                (x => x.Book.Code == bookCode && x.IsBeingReturned == false);
        }

        public async Task<bool> CheckUserSubscription(string registerCode)
        {
            var user = await _context.Users.Include(x => x.Subscription)
                .FirstOrDefaultAsync(x => x.RegisterCode == registerCode);

            if (user != null)
            {
                if (user.Subscription != null &&
                    DateTime.Now <= user.Subscription.EndDate)
                {
                    return true;
                }
                return false;
            }

            return false;
        }
    }
}

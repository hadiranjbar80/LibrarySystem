using Application.Core;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Persistence;

namespace Application.Subscription.Commands
{
    public class Create
    {
        public class Command : IRequest<Result<string>> { }

        public class Handler : IRequestHandler<Command, Result<string>>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;
            private readonly UserManager<AppUser> _userManager;

            public Handler(DataContext context, IUserAccessor userAccessor, 
                UserManager<AppUser> userManager)
            {
                _context = context;
                _userAccessor = userAccessor;
                _userManager = userManager;
            }

            public async Task<Result<string>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByIdAsync(_userAccessor.GetUserId());

                if (user == null) return Result<string>.Failure("User was not found!");

                var subscription = new Domain.Entities.Subscription
                {
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddMonths(12),
                    IsFinally = false,
                    AppUser = user,
                    UserId = user.Id
                };

                _context.Subscriptions.Add(subscription);

                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<string>.Failure("Failed to connect to the payment gateway!");

                return Result<string>.Success(subscription.Id.ToString());
            }
        }
    }
}

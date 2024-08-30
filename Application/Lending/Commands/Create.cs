using Application.Core;
using Application.Interfaces;
using Application.Lending.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Lending.Commands
{
    public class Create
    {
        public class Command : IRequest<Result<Unit>>
        {
            public AddLendingDto LendingDto { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext context, IUserAccessor userAccessor)
            {
                _context = context;
                _userAccessor = userAccessor;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _userAccessor.GetUserByRegisterCode(request.LendingDto.RegisterCode);
                if (user == null) return Result<Unit>.Failure("User was not found");

                var book = await _context.Books.FirstOrDefaultAsync(x => x.Code == request.LendingDto.BookCode);
                if (book == null) return Result<Unit>.Failure("Couldn't find any book with the given id");

                _context.Lendings.Add(new Domain.Entities.Lending
                {
                    AppUser = user,
                    Book = book,
                    IsBeingReturned = false,
                    LendedAt = request.LendingDto.LendedAt,
                    ReturnAt = request.LendingDto.ReturnAt,
                });

                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Unable to add the lending to the database");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}

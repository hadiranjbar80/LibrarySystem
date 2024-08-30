using Application.Core;
using MediatR;
using Persistence;

namespace Application.Lending.Commands
{
    public class Edit
    {
        public class Command : IRequest<Result<Unit>> 
        {
            public Guid Id { get; set; }
            public bool IsBeingReturned { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var lending = await _context.Lendings.FindAsync(request.Id);

                if (lending == null) return Result<Unit>.Failure("Nothing found with the given id");

                lending.IsBeingReturned = request.IsBeingReturned;

                var result = await _context.SaveChangesAsync() > 0;

                if(!result) return Result<Unit>.Failure("Problem updating the information");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}

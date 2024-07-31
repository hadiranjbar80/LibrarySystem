using Application.Core;
using MediatR;
using Persistence;

namespace Application.Book.Commands
{
    public class Edit
    {
        public class Command : IRequest<Result<Unit>> { }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }
            public Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}

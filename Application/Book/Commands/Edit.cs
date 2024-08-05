using Application.Book.Dtos;
using Application.Core;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Book.Commands
{
    public class Edit
    {
        public class Command : IRequest<Result<Unit>> 
        {
            public BookDto Book { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var book = await _context.Books.Include(x=>x.Category).FirstOrDefaultAsync(x=>x.Id == request.Book.Id);

                if (book == null) return Result<Unit>.Failure("Failed find the book");

                _mapper.Map(request.Book, book);

                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to edit the book");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}

using Application.Book.Dtos;
using Application.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Book.Commands
{
    public class Create
    {
        public class Command : IRequest<Result<Unit>>
        {
            public BookDto Book { get; set; }
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
                var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == request.Book.CategoryId);

                if (category == null) return Result<Unit>.Failure("Problem adding book");

                if (_context.Books.Any(x => x.Code == request.Book.Code)) 
                    return Result<Unit>.Failure("The given code is already exist!");

                _context.Books.Add(new Domain.Entities.Book
                {
                    Author = request.Book.Author,
                    Category = category,
                    Name = request.Book.Name,
                    Publisher = request.Book.Publisher,
                    Code = request.Book.Code,
                });

                var result = await _context.SaveChangesAsync() > 0;
                
                if(!result) return Result<Unit>.Failure("Failed to add the book");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}

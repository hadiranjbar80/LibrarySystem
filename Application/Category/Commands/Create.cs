using Application.Category.Dtos;
using Application.Core;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Category.Commands
{
    public class Create
    {
        public class Command : IRequest<Result<Unit>>
        {
            public CategoryDto Category { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x=>x.Category.Name).NotEmpty();
            }
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
                _context.Categories.Add(new Domain.Entities.Category
                {
                    Name = request.Category.Name
                });

                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to create category!");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}

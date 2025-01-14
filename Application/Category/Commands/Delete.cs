﻿using Application.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Category.Commands
{
    public class Delete
    {
        public class Command :IRequest<Result<Unit>>
        {
            public Guid Id { get; set; }
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
                var result = await _context.Categories.Where(x=>x.Id == request.Id)
                    .ExecuteDeleteAsync();

                await _context.SaveChangesAsync();

                if (result > 0) return Result<Unit>.Success(Unit.Value);

                return Result<Unit>.Failure("Failed to delete the category");
            }
        }
    }
}

using Application.Book.Dtos;
using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Book.Queries
{
    public class Details
    {
        public class Query : IRequest<Result<BookDto>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<BookDto>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<Result<BookDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var book = await _context.Books
                    .Include(x=>x.Category)
                    .ProjectTo<BookDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(x=>x.Id == request.Id);

                if (book == null) return Result<BookDto>.Failure("Failed get the book");

                return Result<BookDto>.Success(book);
            }
        }
    }
}

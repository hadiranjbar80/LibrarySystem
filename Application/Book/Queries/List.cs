﻿using Application.Book.Dtos;
using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Book.Queries
{
    public class List
    {
        public class Query : IRequest<Result<List<BookListDto>>> 
        {
            public string SearchQuery { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<BookListDto>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<Result<List<BookListDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var books = await _context.Books
                    .Include(x=>x.Category)
                    .ProjectTo<BookListDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                if (request.SearchQuery != null) 
                {
                    books = books.Where(b => b.Name.Contains(request.SearchQuery) || b.Code == request.SearchQuery).ToList();
                }

                return Result<List<BookListDto>>.Success(books);
            }
        }
    }
}

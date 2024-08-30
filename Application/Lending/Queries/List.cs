using Application.Core;
using Application.Lending.Dtos;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Lending.Queries
{
    public class List
    {
        public class Query : IRequest<Result<List<LendingListDto>>> { }

        public class Handler : IRequestHandler<Query, Result<List<LendingListDto>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result<List<LendingListDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = await _context.Lendings
                    .ProjectTo<LendingListDto>(_mapper.ConfigurationProvider)
                    .AsNoTracking()
                    .ToListAsync();

                return Result<List<LendingListDto>>.Success(query);
            }
        }
    }
}

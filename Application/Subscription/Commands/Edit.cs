using Application.Core;
using Application.Subscription.Dtos;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Subscription.Commands
{
    public class Edit
    {
        public class Command : IRequest<Result<Unit>> 
        {
            public SubscriptionDto Subscription { get; set; }
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
                var subscription = await _context.Subscriptions.FirstOrDefaultAsync(x=>x.Id == request.Subscription.Id);

                if (subscription == null) return Result<Unit>.Failure("Failed to find the subscription");

                _mapper.Map(request.Subscription, subscription);

                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to subscribe!");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}

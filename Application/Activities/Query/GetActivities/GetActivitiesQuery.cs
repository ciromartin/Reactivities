using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Activities.Query.GetActivity;
using Application.Common.Core;
using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Activities.Query.GetActivities
{
    public class GetActivitiesQuery : IRequest<Result<List<ActivityDto>>> { }

    public class Handler : IRequestHandler<GetActivitiesQuery, Result<List<ActivityDto>>>
    {

        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IApplicationDbContext context, IMapper mapper)
        {
            this._mapper = mapper;
            this._context = context;
        }
        public async Task<Result<List<ActivityDto>>> Handle(GetActivitiesQuery request, CancellationToken cancellationToken)
        {
            return Result<List<ActivityDto>>.Success(await _context.Activities
                .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider)
                .ToListAsync());
        }
    }
}
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Activities.Query.GetActivity;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities.Query.GetActivities
{
    public class GetActivitiesQuery : IRequest<List<ActivityDto>> { }

    public class Handler : IRequestHandler<GetActivitiesQuery, List<ActivityDto>>
    {

        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public Handler(DataContext context, IMapper mapper)
        {
            this._mapper = mapper;
            this._context = context;
        }
        public async Task<List<ActivityDto>> Handle(GetActivitiesQuery request, CancellationToken cancellationToken)
        {
            return await _context.Activities
                .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }
    }
}
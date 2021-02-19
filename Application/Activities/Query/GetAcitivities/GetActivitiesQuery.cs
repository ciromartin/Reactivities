using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities.Query.GetAcitivities
{
    public class GetActivitiesQuery : IRequest<List<Activity>> { }

    public class Handler : IRequestHandler<GetActivitiesQuery, List<Activity>>
    {
        private readonly DataContext _context;
        public Handler(DataContext context)
        {
            this._context = context;
        }

        public async Task<List<Activity>> Handle(GetActivitiesQuery request, CancellationToken cancellationToken)
        {
            return await _context.Activities.ToListAsync();
        }
    }

}
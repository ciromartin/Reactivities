using System;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Persistence;

namespace Application.Activities.Query.GetActivity
{
    public class GetActivityQuery : IRequest<Activity>
    {
        public Guid Id { get; set; }
    }

    public class Handler : IRequestHandler<GetActivityQuery, Activity>
    {
        private readonly DataContext _context;
        public Handler(DataContext context)
        {
            this._context = context;
        }

        public async Task<Activity> Handle(GetActivityQuery request, CancellationToken cancellationToken)
        {
            return await _context.Activities.FindAsync(request.Id);
        }
    }
}
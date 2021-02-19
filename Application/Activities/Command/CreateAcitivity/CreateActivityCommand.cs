using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Persistence;

namespace Application.Activities.Command.CreateActivity
{
    public class CreateActivityCommand : IRequest
    {
        public Activity Activity { get; set; }
    }
    public class Handler : IRequestHandler<CreateActivityCommand>
    {
        private readonly DataContext _context;
        public Handler(DataContext context)
        {
            this._context = context;
        }

        public async Task<Unit> Handle(CreateActivityCommand request, CancellationToken cancellationToken)
        {
            _context.Activities.Add(request.Activity);

            await _context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}

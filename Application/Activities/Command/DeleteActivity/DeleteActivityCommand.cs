using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Persistence;

namespace Application.Activities.Command.DeleteActivity
{
    public class DeleteActivityCommand : IRequest
    {
        public Guid Id { get; set; }
    }

    public class Handler : IRequestHandler<DeleteActivityCommand>
    {
        private readonly DataContext _context;
        public Handler(DataContext context)
        {
            this._context = context;
        }

        public async Task<Unit> Handle(DeleteActivityCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Activities.FindAsync(request.Id);

            _context.Remove(entity);

            await _context.SaveChangesAsync();

            return Unit.Value;
        }
    }

}
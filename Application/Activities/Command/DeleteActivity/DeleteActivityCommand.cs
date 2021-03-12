using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Core;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Activities.Command.DeleteActivity
{
    public class DeleteActivityCommand : IRequest<Result<Unit>>
    {
        public Guid Id { get; set; }
    }

    public class Handler : IRequestHandler<DeleteActivityCommand, Result<Unit>>
    {
        private readonly IApplicationDbContext _context;
        public Handler(IApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<Result<Unit>> Handle(DeleteActivityCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Activities.FindAsync(request.Id);

            if (entity == null) return Result<Unit>.Failure(new Error { Title = nameof(DeleteActivityCommand), Description = $"doesn't exists activity id: '{request.Id}'" });

            _context.Activities.Remove(entity);

            var success = await _context.SaveChangesAsync(cancellationToken) > 0;

            if(!success) return Result<Unit>.Failure(new Error { Title = nameof(DeleteActivityCommand), Description = $"couldn't delete activity id: '{request.Id}'"});

            return Result<Unit>.Success(Unit.Value);
        }
    }

}
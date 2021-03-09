using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Core;
using MediatR;
using Persistence;

namespace Application.Activities.Command.DeleteActivity
{
    public class DeleteActivityCommand : IRequest<Result<Unit>>
    {
        public Guid Id { get; set; }
    }

    public class Handler : IRequestHandler<DeleteActivityCommand, Result<Unit>>
    {
        private readonly DataContext _context;
        public Handler(DataContext context)
        {
            this._context = context;
        }

        public async Task<Result<Unit>> Handle(DeleteActivityCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Activities.FindAsync(request.Id);

            if (entity == null) return Result<Unit>.Failure(new Error { Title = nameof(DeleteActivityCommand), Description = $"doesn't exists activity id: '{request.Id}'" });

            _context.Remove(entity);

            var success = await _context.SaveChangesAsync() > 0;

            if(!success) return Result<Unit>.Failure(new Error { Title = nameof(DeleteActivityCommand), Description = $"couldn't delete activity id: '{request.Id}'"});

            return Result<Unit>.Success(Unit.Value);
        }
    }

}
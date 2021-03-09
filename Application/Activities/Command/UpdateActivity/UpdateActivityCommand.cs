using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Core;
using Application.Common.Mappings;
using AutoMapper;
using Domain;
using MediatR;
using Persistence;

namespace Application.Activities.Command.UpdateActivity
{
    public class UpdateActivityCommand : IRequest<Result<Unit>>, IMapTo<Activity>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string City { get; set; }
        public string Venue { get; set; }

    }

    public class Handler : IRequestHandler<UpdateActivityCommand, Result<Unit>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public Handler(DataContext context, IMapper mapper)
        {
            this._mapper = mapper;
            this._context = context;
        }

        public async Task<Result<Unit>> Handle(UpdateActivityCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Activities.FindAsync(request.Id);

            if (entity == null) return Result<Unit>.Failure(new Error { Title = nameof(UpdateActivityCommand), Description = $"doesn't exists activity id: '{request.Id}'"});

            _mapper.Map(request, entity);

            var success = await _context.SaveChangesAsync() > 0;

            if(!success) return Result<Unit>.Failure(new Error { Title = nameof(UpdateActivityCommand), Description = $"couldn't update activity id: '{request.Id}'"});

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
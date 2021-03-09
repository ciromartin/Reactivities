using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Core;
using Application.Common.Mappings;
using AutoMapper;
using Domain;
using MediatR;
using Persistence;

namespace Application.Activities.Command.CreateActivity
{
    public class CreateActivityCommand : IRequest<Result<Guid>>, IMapTo<Activity>
    {
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string City { get; set; }
        public string Venue { get; set; }
    }

    public class Handler : IRequestHandler<CreateActivityCommand, Result<Guid>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public Handler(DataContext context, IMapper mapper)
        {
            this._mapper = mapper;
            this._context = context;
        }

        public async Task<Result<Guid>> Handle(CreateActivityCommand request, CancellationToken cancellationToken)
        {
            var entity = new Activity();

            _mapper.Map(request, entity);

            _context.Activities.Add(entity);

            var success = await _context.SaveChangesAsync() > 0;

            if(!success) return Result<Guid>.Failure(new Error() { Title = nameof(CreateActivityCommand), Description = "Activity not created"});

            return Result<Guid>.Success(entity.Id);            
        }
    }
}

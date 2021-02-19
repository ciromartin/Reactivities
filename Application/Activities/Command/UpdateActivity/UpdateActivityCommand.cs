using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Mappings;
using AutoMapper;
using Domain;
using MediatR;
using Persistence;

namespace Application.Activities.Command.UpdateActivity
{
    public class UpdateActivityCommand : IRequest, IMapTo<Activity>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string City { get; set; }
        public string Venue { get; set; }

    }

    public class Handler : IRequestHandler<UpdateActivityCommand>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public Handler(DataContext context, IMapper mapper)
        {
            this._mapper = mapper;
            this._context = context;
        }

        public async Task<Unit> Handle(UpdateActivityCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Activities.FindAsync(request.Id);

            _mapper.Map(request, entity);

            await _context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
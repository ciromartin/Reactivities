using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Core;
using AutoMapper;
using MediatR;
using Persistence;

namespace Application.Activities.Query.GetActivity
{
    public class GetActivityQuery : IRequest<Result<ActivityDto>>
    {
        public Guid Id { get; set; }
    }

    public class Handler : IRequestHandler<GetActivityQuery, Result<ActivityDto>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public Handler(DataContext context, IMapper mapper)
        {
            this._mapper = mapper;
            this._context = context;
        }

        public async Task<Result<ActivityDto>> Handle(GetActivityQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Activities
                .FindAsync(request.Id);

            return Result<ActivityDto>.Success(_mapper.Map<ActivityDto>(entity));
        }
    }
}
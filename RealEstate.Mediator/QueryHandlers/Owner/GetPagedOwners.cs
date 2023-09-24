using AutoMapper;
using DTOModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RealEstate.Mediator.Query.Owner;

namespace RealEstate.Mediator.QueryHandlers.Owner
{
    internal class GetPagedOwnersCommandHandler : IRequestHandler<GetPagedOwnersQuery, IEnumerable<OwnerDto>>
    {
        private readonly RealEstateDbContext _context;
        private readonly IMapper _mapper;

        public GetPagedOwnersCommandHandler(RealEstateDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OwnerDto>> Handle(GetPagedOwnersQuery request, CancellationToken cancellationToken)
        {
            List<DataModels.Owner> owners = await _context.Owners
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            return _mapper.Map<IEnumerable<OwnerDto>>(owners);
        }
    }
}

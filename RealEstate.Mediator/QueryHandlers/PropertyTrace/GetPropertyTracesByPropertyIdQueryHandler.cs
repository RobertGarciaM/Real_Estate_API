using AutoMapper;
using DTOModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RealEstate.Mediator.Query.PropertyTrace;

namespace RealEstate.Mediator.QueryHandlers.PropertyTrace
{
    internal class GetPropertyTracesByPropertyIdQueryHandler : IRequestHandler<GetPropertyTracesByPropertyIdQuery, IEnumerable<PropertyTraceDto>>
    {
        private readonly RealEstateDbContext _context;
        private readonly IMapper _mapper;

        public GetPropertyTracesByPropertyIdQueryHandler(RealEstateDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PropertyTraceDto>> Handle(GetPropertyTracesByPropertyIdQuery request, CancellationToken cancellationToken)
        {
            List<DataModels.PropertyTrace> propertyTraces = await _context.PropertyTraces
                .Where(pt => pt.IdProperty == request.PropertyId)
                .ToListAsync(cancellationToken);

            return _mapper.Map<IEnumerable<PropertyTraceDto>>(propertyTraces);
        }
    }
}
using AutoMapper;
using DTOModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RealEstate.Mediator.Query.PropertyTrace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Mediator.QueryHandlers.PropertyTrace
{
    internal class GetPropertyTracesByPropertyIdQueryHandler : IRequestHandler<GetPropertyTracesByPropertyIdQuery, IEnumerable<PropertyTraceDto>>
    {
        private readonly InMemoryDbContext _context;
        private readonly IMapper _mapper;

        public GetPropertyTracesByPropertyIdQueryHandler(InMemoryDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PropertyTraceDto>> Handle(GetPropertyTracesByPropertyIdQuery request, CancellationToken cancellationToken)
        {
            var propertyTraces = await _context.PropertyTraces
                .Where(pt => pt.IdProperty == request.PropertyId)
                .ToListAsync(cancellationToken);

            return _mapper.Map<IEnumerable<PropertyTraceDto>>(propertyTraces);
        }
    }
}
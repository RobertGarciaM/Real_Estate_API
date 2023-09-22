using AutoMapper;
using DataModels;
using DTOModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RealEstate.Mediator.Query.PropertyCommand;

namespace RealEstate.Mediator.QueryHandlers.PropertyQuery
{
    internal class GetPropertiesQueryHandler : IRequestHandler<GetPropertiesQuery, IEnumerable<PropertyDto>>
    {
        private readonly InMemoryDbContext _context;
        private readonly IMapper _mapper;

        public GetPropertiesQueryHandler(InMemoryDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PropertyDto>> Handle(GetPropertiesQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Property> query = _context.Properties;

            if (request.MinPrice > 0 || request.MaxPrice > 0)
            {
                query = query.Where(property =>
                    (request.MinPrice <= 0 || property.Price >= request.MinPrice) &&
                    (request.MaxPrice <= 0 || property.Price <= request.MaxPrice)
                );
            }

            if (request.Year > 0)
            {
                query = query.Where(property => property.Year == request.Year);
            }

            if (!string.IsNullOrEmpty(request.Name))
            {
                query = query.Where(property => property.Name.ToLower().Contains(request.Name.ToLower()));
            }

            if (!string.IsNullOrEmpty(request.Address))
            {
                query = query.Where(property => property.Address.ToLower().Contains(request.Address.ToLower()));
            }

            if (!string.IsNullOrEmpty(request.CodeInternal))
            {
                query = query.Where(property => property.CodeInternal.ToLower() == request.CodeInternal.ToLower());
            }

            if (request.IdOwner != Guid.Empty)
            {
                query = query.Where(property => property.IdOwner == request.IdOwner);
            }

            int skip = (request.Page - 1) * request.PageSize;
            List<Property> result = await query
                .Skip(skip)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<PropertyDto>>(result);
        }
    }
}
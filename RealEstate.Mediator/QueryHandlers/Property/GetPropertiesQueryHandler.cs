using AutoMapper;
using DataModels;
using DTOModels;
using LinqKit;
using MediatR;
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

            ExpressionStarter<Property> predicate = PredicateBuilder.New<Property>(property => false);

            if (request.Price > 0)
            {
                predicate = predicate.Or(property => property.Price == request.Price);
            }

            if (request.Year > 0)
            {
                predicate = predicate.Or(property => property.Year == request.Year);
            }

            if (!string.IsNullOrEmpty(request.Name))
            {
                predicate = predicate.Or(property => property.Name.ToLower().Contains(request.Name.ToLower()));
            }

            if (!string.IsNullOrEmpty(request.Address))
            {
                predicate = predicate.Or(property => property.Address.ToLower().Contains(request.Address.ToLower()));
            }

            if (!string.IsNullOrEmpty(request.CodeInternal))
            {
                predicate = predicate.Or(property => property.CodeInternal.ToLower() == request.CodeInternal.ToLower());
            }

            if (request.IdOwner != Guid.Empty)
            {
                predicate = predicate.Or(property => property.IdOwner == request.IdOwner);
            }

            int skip = (request.Page - 1) * request.PageSize;

            // TODO: This is not optimal and should only be used in very small applications, we will change it in the version where we will use SQL Server.
            List<Property> result = query
              .AsExpandable()
              .Where(predicate)
              .ToList();

            return result
                .Skip(skip)
                .Take(request.PageSize)
                .Select(property => _mapper.Map<PropertyDto>(property))
                .ToList();
        }
    }
}
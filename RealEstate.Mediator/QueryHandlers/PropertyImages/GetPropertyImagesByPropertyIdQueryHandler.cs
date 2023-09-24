using AutoMapper;
using DTOModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RealEstate.Mediator.Query.PropertyImages;

namespace RealEstate.Mediator.QueryHandlers.PropertyImages
{
    internal class GetPropertyImagesByPropertyIdQueryHandler : IRequestHandler<GetPropertyImagesByPropertyIdQuery, IEnumerable<PropertyImageDto>>
    {
        private readonly RealEstateDbContext _context;
        private readonly IMapper _mapper;

        public GetPropertyImagesByPropertyIdQueryHandler(RealEstateDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PropertyImageDto>> Handle(GetPropertyImagesByPropertyIdQuery request, CancellationToken cancellationToken)
        {
            int skip = (request.Page - 1) * request.PageSize;
            List<DataModels.PropertyImage> propertyImages = await _context.PropertyImages
                .Where(pi => pi.IdProperty == request.PropertyId)
                .Skip(skip)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            return _mapper.Map<IEnumerable<PropertyImageDto>>(propertyImages);
        }
    }
}

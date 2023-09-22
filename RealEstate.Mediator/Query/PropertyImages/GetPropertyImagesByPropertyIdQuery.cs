using DTOModels;
using MediatR;

namespace RealEstate.Mediator.Query.PropertyImages
{
    public record GetPropertyImagesByPropertyIdQuery : IRequest<IEnumerable<PropertyImageDto>>
    {
        public Guid PropertyId { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }

        public GetPropertyImagesByPropertyIdQuery(Guid propertyId, int page, int pageSize)
        {
            PropertyId = propertyId;
            Page = page > 0 ? page : 1;
            PageSize = pageSize > 0 ? pageSize : 10;
        }
    }
}

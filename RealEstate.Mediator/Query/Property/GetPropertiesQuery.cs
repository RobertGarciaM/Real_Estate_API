using DTOModels;
using MediatR;

namespace RealEstate.Mediator.Query.PropertyCommand
{
    public class GetPropertiesQuery : IRequest<IEnumerable<PropertyDto>>
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public decimal Price { get; set; }
        public int Year { get; set; }
        public string? CodeInternal { get; set; }
        public Guid IdOwner { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
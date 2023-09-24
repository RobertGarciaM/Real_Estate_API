using MediatR;

namespace RealEstate.Mediator.Query.PropertyCommand
{
    public record CheckPropertyExistsCommand : IRequest<bool>
    {
        public Guid PropertyId { get; set; }

        public CheckPropertyExistsCommand(Guid propertyId)
        {
            PropertyId = propertyId;
        }
    }
}

using MediatR;

namespace RealEstate.Mediator.Query.Owner
{
    public record CheckOwnerExistsQuery : IRequest<bool>
    {
        public Guid OwnerId { get; set; }

        public CheckOwnerExistsQuery(Guid ownerId)
        {
            OwnerId = ownerId;
        }
    }
}

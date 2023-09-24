using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace RealEstate.Mediator.Commands.Owner
{
    public record DeleteOwnerCommand : IRequest<ActionResult>
    {
        public Guid OwnerId { get; set; }
    }
}

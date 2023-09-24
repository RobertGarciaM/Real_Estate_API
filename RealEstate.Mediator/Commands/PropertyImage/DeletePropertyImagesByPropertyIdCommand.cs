using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace RealEstate.Mediator.Commands.PropertyImageCommand
{
    public record DeletePropertyImagesByPropertyIdCommand : IRequest<ActionResult>
    {
        public Guid PropertyId { get; set; }
    }
}

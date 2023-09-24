using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace RealEstate.Mediator.Commands.PropertyImageCommand
{
    public record DeletePropertyImageCommand : IRequest<ActionResult>
    {
        public Guid PropertyImageId { get; set; }
    }
}

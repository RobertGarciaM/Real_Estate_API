using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace RealEstate.Mediator.Commands.PropertyCommand
{
    public record DeletePropertyCommand : IRequest<ActionResult>
    {
        public Guid PropertyId { get; set; }
    }
}

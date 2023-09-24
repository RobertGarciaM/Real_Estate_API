using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace RealEstate.Mediator.Commands.PropertyTraceCommand
{
    public record DeletePropertyTraceCommand : IRequest<ActionResult>
    {
        public Guid PropertyTraceId { get; set; }
    }
}

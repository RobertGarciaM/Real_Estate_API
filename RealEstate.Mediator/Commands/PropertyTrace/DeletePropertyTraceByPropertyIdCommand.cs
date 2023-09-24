using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace RealEstate.Mediator.Commands.PropertyTraceCommand
{
    public record DeletePropertyTraceByPropertyIdCommand : IRequest<ActionResult>
    {
        public Guid PropertyId { get; set; }
    }
}

using DTOModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace RealEstate.Mediator.Commands.PropertyTraceCommand
{
    public record UpdatePropertyTraceCommand : IRequest<ActionResult>
    {
        public UpdatePropertyTraceDto? dto { get; set; }
    }
}

using DTOModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace RealEstate.Mediator.Commands.PropertyTraceCommand
{
    public record CreatePropertyTraceCommand : IRequest<ActionResult>
    {
        public CreatePropertyTraceCommand(CreatePropertyTraceDto dto)
        {
            Dto = dto;
        }

        public CreatePropertyTraceDto Dto { get; }
    }
}

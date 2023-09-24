using DTOModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace RealEstate.Mediator.Commands.PropertyCommand
{
    public record CreatePropertyCommand : IRequest<ActionResult>
    {
        public CreatePropertyCommand(CreatePropertyDto dto)
        {
            Dto = dto;
        }

        public CreatePropertyDto Dto { get; }
    }
}

using DTOModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace RealEstate.Mediator.Commands.PropertyImage
{
    public record CreatePropertyImageCommand : IRequest<ActionResult>
    {
        public CreatePropertyImageCommand(CreatePropertyImageDto dto)
        {
            Dto = dto;
        }
        public CreatePropertyImageDto Dto { get; set; }
    }
}

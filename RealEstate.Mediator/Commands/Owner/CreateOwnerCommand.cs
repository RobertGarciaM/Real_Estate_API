using DTOModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace RealEstate.Mediator.Commands.Owner
{
    public record CreateOwnerCommand : IRequest<ActionResult>
    {
        public CreateOwnerCommand(CreateOwnerDto dto)
        {
            Dto = dto;
        }

        public CreateOwnerDto Dto { get; }
    }
}

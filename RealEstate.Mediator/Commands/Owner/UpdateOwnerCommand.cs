using DTOModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace RealEstate.Mediator.Commands.Owner
{
    public record UpdateOwnerCommand : IRequest<ActionResult>
    {
        public UpdateOwnerDto? UpdateDto { get; set; }
    }
}

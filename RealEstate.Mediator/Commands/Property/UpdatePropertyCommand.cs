using DTOModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace RealEstate.Mediator.Commands.PropertyCommand
{
    public record UpdatePropertyCommand : IRequest<ActionResult>
    {
        public UpdatePropertyDto? UpdateDto { get; set; }
    }
}
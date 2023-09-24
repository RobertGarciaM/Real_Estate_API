using DTOModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace RealEstate.Mediator.Commands.Owner
{
    public record UpdatePropertyImageCommand : IRequest<ActionResult>
    {
        public UpdatePropertyImagesDto? dto { get; set; }
    }
}

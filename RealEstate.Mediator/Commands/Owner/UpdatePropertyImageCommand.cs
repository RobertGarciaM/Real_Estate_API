using DTOModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Mediator.Commands.Owner
{
    public record UpdatePropertyImageCommand : IRequest<ActionResult>
    {
        public UpdatePropertyImagesDto dto { get; set; }
    }
}

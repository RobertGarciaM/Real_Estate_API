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
    public record UpdateOwnerCommand : IRequest<ActionResult>
    {
        public Guid OwnerId { get; set; }
        public UpdateOwnerDto UpdateDto { get; set; }
    }
}

using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Mediator.Commands.PropertyImageCommand
{
    public struct DeletePropertyImagesByPropertyIdCommand : IRequest<ActionResult>
    {
        public Guid PropertyId { get; set; }
    }
}

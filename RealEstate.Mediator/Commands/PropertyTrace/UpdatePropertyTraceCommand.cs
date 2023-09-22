using DTOModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Mediator.Commands.PropertyTraceCommand
{
    public record UpdatePropertyTraceCommand : IRequest<ActionResult>
    {
        public UpdatePropertyTraceDto dto { get; set; }
    }
}

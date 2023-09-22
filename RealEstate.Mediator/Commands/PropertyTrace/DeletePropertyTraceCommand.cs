using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Mediator.Commands.PropertyTraceCommand
{
    public record DeletePropertyTraceCommand : IRequest<ActionResult>
    {
        public Guid PropertyTraceId { get; set; }
    }
}

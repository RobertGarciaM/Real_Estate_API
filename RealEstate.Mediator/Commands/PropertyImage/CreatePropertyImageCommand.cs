using DTOModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Mediator.Commands.PropertyImage
{
    public record CreatePropertyImageCommand : IRequest<ActionResult>
    {
        public CreatePropertyImageCommand(CreatePropertyImageDto dto) { 
            Dto= dto;
        }
        public CreatePropertyImageDto Dto { get; set; }
    }
}

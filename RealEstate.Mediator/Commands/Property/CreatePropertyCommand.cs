using DTOModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Mediator.Commands.Property
{
    public class CreatePropertyCommand : IRequest<ActionResult>
    {
        public CreatePropertyCommand(CreatePropertyDto dto)
        {
            Dto = dto;
        }

        public CreatePropertyDto Dto { get; }
    }
}

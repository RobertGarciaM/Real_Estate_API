using AutoMapper;
using DataModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Mediator.Commands.Owner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Mediator.CommandHandlers.PropertyImageHandler
{
    internal class UpdatePropertyImageCommandHandler : IRequestHandler<UpdatePropertyImageCommand, ActionResult>
    {
        private readonly InMemoryDbContext _context;
        private readonly IMapper _mapper;

        public UpdatePropertyImageCommandHandler(InMemoryDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ActionResult> Handle(UpdatePropertyImageCommand request, CancellationToken cancellationToken)
        {
            var existingPropertyImage = await _context.PropertyImages.FindAsync(request.dto.PropertyImageId);

            if (existingPropertyImage == null)
            {
                return new NotFoundResult();
            }

            _mapper.Map(request.dto, existingPropertyImage);

            await _context.SaveChangesAsync();

            return new OkResult();
        }
    }
}
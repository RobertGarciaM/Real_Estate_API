using AutoMapper;
using DataModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Mediator.Commands.Owner;

namespace RealEstate.Mediator.CommandHandlers.PropertyImageHandler
{
    internal class UpdatePropertyImageCommandHandler : IRequestHandler<UpdatePropertyImageCommand, ActionResult>
    {
        private readonly RealEstateDbContext _context;
        private readonly IMapper _mapper;

        public UpdatePropertyImageCommandHandler(RealEstateDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ActionResult> Handle(UpdatePropertyImageCommand request, CancellationToken cancellationToken)
        {
            PropertyImage? existingPropertyImage = await _context.PropertyImages.FindAsync(request.dto.PropertyImageId);

            if (existingPropertyImage == null)
            {
                return new NotFoundResult();
            }

            _ = _mapper.Map(request.dto, existingPropertyImage);

            _ = await _context.SaveChangesAsync();

            return new OkResult();
        }
    }
}
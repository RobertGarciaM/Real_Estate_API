using AutoMapper;
using DataModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Mediator.Commands.PropertyImage;
using RealEstate.Mediator.Query.PropertyCommand;

namespace RealEstate.Mediator.CommandHandlers.PropertyImageHandler
{
    internal class CreatePropertyImageCommandHandler : IRequestHandler<CreatePropertyImageCommand, ActionResult>
    {
        private readonly InMemoryDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public CreatePropertyImageCommandHandler(InMemoryDbContext repository, IMapper mapper, IMediator mediator)
        {
            _context = repository;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<ActionResult> Handle(CreatePropertyImageCommand request, CancellationToken cancellationToken)
        {
            bool propertyExists = await _mediator.Send(new CheckPropertyExistsCommand(request.Dto.IdProperty));

            if (!propertyExists)
            {
                return new NotFoundObjectResult(new { Message = "The Property does not exists." });
            }

            PropertyImage propertyImage = _mapper.Map<PropertyImage>(request.Dto);

            _ = await _context.AddAsync(propertyImage);
            _ = await _context.SaveChangesAsync();


            return new OkObjectResult(new { Id = propertyImage.IdPropertyImage });
        }
    }
}
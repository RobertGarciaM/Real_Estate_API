using AutoMapper;
using DataModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Mediator.Commands.Owner;
using RealEstate.Mediator.Commands.Property;

namespace RealEstate.Mediator.Handlers.PropertyHandler
{
    internal class CreatePropertyCommandHandler : IRequestHandler<CreatePropertyCommand, ActionResult>
    {
        private readonly InMemoryDbContext _context;
        private readonly IMapper _mapper;

        public CreatePropertyCommandHandler(InMemoryDbContext repository, IMapper mapper)
        {
            _context = repository;
            _mapper = mapper;
        }

        public async Task<ActionResult> Handle(CreatePropertyCommand request, CancellationToken cancellationToken)
        {
            var property = _mapper.Map<Property>(request.Dto);

            _ = await _context.AddAsync(property);
            _ = await _context.SaveChangesAsync();


            return new OkObjectResult(new { Id = property.IdProperty });
        }
    }
}

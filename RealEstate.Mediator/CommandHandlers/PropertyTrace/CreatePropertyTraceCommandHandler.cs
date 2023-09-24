using AutoMapper;
using DataModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Mediator.Commands.PropertyTraceCommand;

namespace RealEstate.Mediator.CommandHandlers.PropertyTraceHandler
{
    internal class CreatePropertyTraceCommandHandler : IRequestHandler<CreatePropertyTraceCommand, ActionResult>
    {
        private readonly RealEstateDbContext _context;
        private readonly IMapper _mapper;

        public CreatePropertyTraceCommandHandler(RealEstateDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ActionResult> Handle(CreatePropertyTraceCommand request, CancellationToken cancellationToken)
        {
            PropertyTrace propertyTrace = _mapper.Map<PropertyTrace>(request.Dto);

            propertyTrace.IdPropertyTrace = Guid.NewGuid();

            _ = await _context.AddAsync(propertyTrace);
            _ = await _context.SaveChangesAsync();

            return new OkObjectResult(new { Id = propertyTrace.IdPropertyTrace });
        }
    }
}

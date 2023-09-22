using AutoMapper;
using DataModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstate.Mediator.Commands.PropertyCommand;
using RealEstate.Mediator.Query.Owner;

namespace RealEstate.Mediator.CommandHandlers.PropertyHandler
{
    internal class UpdatePropertyCommandHandler : IRequestHandler<UpdatePropertyCommand, ActionResult>
    {
        private readonly InMemoryDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public UpdatePropertyCommandHandler(InMemoryDbContext context, IMapper mapper, IMediator mediator)
        {
            _context = context;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<ActionResult> Handle(UpdatePropertyCommand request, CancellationToken cancellationToken)
        {
            var property = await _context.Properties.FirstOrDefaultAsync(p => p.IdProperty == request.PropertyId, cancellationToken);

            if (property == null)
            {
                return new NotFoundResult();
            }

            var ownerExists = await _mediator.Send(new CheckOwnerExistsQuery(request.UpdateDto.IdOwner));

            if (!ownerExists)
            {
                return new NotFoundObjectResult(new { Message = "The Owner does not exists." });
            }

            _mapper.Map(request.UpdateDto, property);

            await _context.SaveChangesAsync(cancellationToken);

            return new OkResult();
        }
    }
}

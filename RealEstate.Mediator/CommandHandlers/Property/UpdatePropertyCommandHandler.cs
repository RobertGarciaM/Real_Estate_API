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
        private readonly RealEstateDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public UpdatePropertyCommandHandler(RealEstateDbContext context, IMapper mapper, IMediator mediator)
        {
            _context = context;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<ActionResult> Handle(UpdatePropertyCommand request, CancellationToken cancellationToken)
        {
            Property? property = await _context.Properties.FirstOrDefaultAsync(p => p.IdProperty == request.UpdateDto.Id, cancellationToken);

            if (property == null)
            {
                return new NotFoundResult();
            }

            bool ownerExists = await _mediator.Send(new CheckOwnerExistsQuery(request.UpdateDto.IdOwner));

            if (!ownerExists)
            {
                return new NotFoundObjectResult(new { Message = "The Owner does not exists." });
            }

            _ = _mapper.Map(request.UpdateDto, property);

            _ = await _context.SaveChangesAsync(cancellationToken);

            return new OkResult();
        }
    }
}

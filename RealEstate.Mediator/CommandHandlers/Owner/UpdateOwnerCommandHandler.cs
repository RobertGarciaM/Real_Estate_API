using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Mediator.Commands.Owner;

namespace RealEstate.Mediator.Handlers.OwnerHandler
{
    internal class UpdateOwnerCommandHandler : IRequestHandler<UpdateOwnerCommand, ActionResult>
    {
        private readonly InMemoryDbContext _context;
        private readonly IMapper _mapper;

        public UpdateOwnerCommandHandler(InMemoryDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ActionResult> Handle(UpdateOwnerCommand request, CancellationToken cancellationToken)
        {
            DataModels.Owner? owner = await _context.Owners.FindAsync(request.UpdateDto.Id);
            if (owner == null)
            {
                return new NotFoundResult();
            }

            _ = _mapper.Map(request.UpdateDto, owner);

            _ = await _context.SaveChangesAsync(cancellationToken);

            return new OkResult();
        }
    }
}
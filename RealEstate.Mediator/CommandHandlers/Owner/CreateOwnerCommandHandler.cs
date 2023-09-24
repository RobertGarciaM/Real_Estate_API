using AutoMapper;
using DataModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Mediator.Commands.Owner;
using RealEstate.Mediator.CustomException;

namespace RealEstate.Mediator.Handlers.OwnerHandler
{
    internal class CreateOwnerCommandHandler : IRequestHandler<CreateOwnerCommand, ActionResult>
    {
        private readonly InMemoryDbContext _context;
        private readonly IMapper _mapper;

        public CreateOwnerCommandHandler(InMemoryDbContext ownerRepository, IMapper mapper)
        {
            _context = ownerRepository;
            _mapper = mapper;
        }

        public async Task<ActionResult> Handle(CreateOwnerCommand request, CancellationToken cancellationToken)
        {
            Owner owner = _mapper.Map<Owner>(request.Dto);

            if (owner == null)
            {
                throw new EntityNullException();
            }

            _ = await _context.AddAsync(owner);
            _ = await _context.SaveChangesAsync();


            return new OkObjectResult(new { Id = owner.IdOwner });
        }
    }
}

using AutoMapper;
using DataModels;
using DTOModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Mediator.Commands.Owner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Mediator.Handlers
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
            var owner = _mapper.Map<Owner>(request.Dto);

            _ = await _context.AddAsync(owner);
            _ = await _context.SaveChangesAsync();


            return new OkObjectResult(new { Id = owner.IdOwner });
        }
    }
}

using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Mediator.Commands.Owner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var owner = await _context.Owners.FindAsync(request.UpdateDto.Id);
            if (owner == null)
            {
                return new NotFoundResult();
            }

            _mapper.Map(request.UpdateDto, owner);

            await _context.SaveChangesAsync(cancellationToken);

            return new OkResult();
        }
    }
}
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
    internal class DeleteOwnerCommandHandler : IRequestHandler<DeleteOwnerCommand, ActionResult>
    {
        private readonly InMemoryDbContext _context;

        public DeleteOwnerCommandHandler(InMemoryDbContext context)
        {
            _context = context;
        }

        public async Task<ActionResult> Handle(DeleteOwnerCommand request, CancellationToken cancellationToken)
        {
            var owner = await _context.Owners.FindAsync(request.OwnerId);
            if (owner == null)
            {
                return new NotFoundResult();
            }

            _context.Owners.Remove(owner);
            await _context.SaveChangesAsync(cancellationToken);

            return new OkResult();
        }
    }
}

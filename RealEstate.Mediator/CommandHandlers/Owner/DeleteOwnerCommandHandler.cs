using MediatR;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Mediator.Commands.Owner;

namespace RealEstate.Mediator.Handlers.OwnerHandler
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
            DataModels.Owner? owner = await _context.Owners.FindAsync(request.OwnerId);
            if (owner == null)
            {
                return new NotFoundResult();
            }

            _ = _context.Owners.Remove(owner);
            _ = await _context.SaveChangesAsync(cancellationToken);

            return new OkResult();
        }
    }
}

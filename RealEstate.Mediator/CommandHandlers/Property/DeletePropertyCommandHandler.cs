using MediatR;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Mediator.Commands.PropertyCommand;

namespace RealEstate.Mediator.CommandHandlers.PropertyHandler
{
    internal class DeletePropertyCommandHandler : IRequestHandler<DeletePropertyCommand, ActionResult>
    {
        private readonly InMemoryDbContext _context;

        public DeletePropertyCommandHandler(InMemoryDbContext context)
        {
            _context = context;
        }

        public async Task<ActionResult> Handle(DeletePropertyCommand request, CancellationToken cancellationToken)
        {
            var property = await _context.Properties.FindAsync(request.PropertyId);
            if (property == null)
            {
                return new NotFoundResult();
            }

            _context.Properties.Remove(property);
            await _context.SaveChangesAsync(cancellationToken);

            return new OkResult();
        }
    }
}
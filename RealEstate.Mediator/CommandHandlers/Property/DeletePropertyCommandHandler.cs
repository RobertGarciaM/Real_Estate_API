using MediatR;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Mediator.Commands.PropertyCommand;
using RealEstate.Mediator.Commands.PropertyImageCommand;

namespace RealEstate.Mediator.CommandHandlers.PropertyHandler
{
    internal class DeletePropertyCommandHandler : IRequestHandler<DeletePropertyCommand, ActionResult>
    {
        private readonly InMemoryDbContext _context;
        private readonly IMediator _mediator;

        public DeletePropertyCommandHandler(InMemoryDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<ActionResult> Handle(DeletePropertyCommand request, CancellationToken cancellationToken)
        {
            var property = await _context.Properties.FindAsync(request.PropertyId);
            if (property == null)
            {
                return new NotFoundResult();
            }

            await _mediator.Send(new DeletePropertyImagesByPropertyIdCommand { PropertyId = request.PropertyId });
            _context.Properties.Remove(property);
            await _context.SaveChangesAsync(cancellationToken);

            return new OkResult();
        }
    }
}
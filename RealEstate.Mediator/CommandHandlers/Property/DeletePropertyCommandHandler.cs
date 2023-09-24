using MediatR;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Mediator.Commands.PropertyCommand;
using RealEstate.Mediator.Commands.PropertyImageCommand;
using RealEstate.Mediator.Commands.PropertyTraceCommand;

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
            DataModels.Property? property = await _context.Properties.FindAsync(request.PropertyId);
            if (property == null)
            {
                return new NotFoundResult();
            }

            _ = await _mediator.Send(new DeletePropertyImagesByPropertyIdCommand { PropertyId = request.PropertyId });
            _ = await _mediator.Send(new DeletePropertyTraceByPropertyIdCommand { PropertyId = request.PropertyId });
            _ = _context.Properties.Remove(property);
            _ = await _context.SaveChangesAsync(cancellationToken);

            return new OkResult();
        }
    }
}
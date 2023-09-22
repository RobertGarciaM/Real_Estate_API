using MediatR;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Mediator.Commands.PropertyTraceCommand;

namespace RealEstate.Mediator.CommandHandlers.PropertyTraceHanlder
{
    internal class DeletePropertyTraceCommandHandler : IRequestHandler<DeletePropertyTraceCommand, ActionResult>
    {
        private readonly InMemoryDbContext _context;
        private readonly IMediator _mediator;

        public DeletePropertyTraceCommandHandler(InMemoryDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<ActionResult> Handle(DeletePropertyTraceCommand request, CancellationToken cancellationToken)
        {
            var propertyTrace = await _context.PropertyTraces.FindAsync(request.PropertyTraceId);
            if (propertyTrace == null)
            {
                return new NotFoundResult();
            }

            _context.PropertyTraces.Remove(propertyTrace);
            await _context.SaveChangesAsync(cancellationToken);

            return new OkResult();
        }
    }
}

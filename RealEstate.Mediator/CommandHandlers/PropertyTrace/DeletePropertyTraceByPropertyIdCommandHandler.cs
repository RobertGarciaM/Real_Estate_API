using MediatR;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Mediator.Commands.PropertyTraceCommand;

namespace RealEstate.Mediator.CommandHandlers.PropertyTraceHandler
{
    internal class DeletePropertyTraceByPropertyIdCommandHandler : IRequestHandler<DeletePropertyTraceByPropertyIdCommand, ActionResult>
    {
        private readonly InMemoryDbContext _context;

        public DeletePropertyTraceByPropertyIdCommandHandler(InMemoryDbContext context)
        {
            _context = context;
        }

        public async Task<ActionResult> Handle(DeletePropertyTraceByPropertyIdCommand request, CancellationToken cancellationToken)
        {
            List<DataModels.PropertyTrace> propertyTraceToDelete = _context.PropertyTraces
                .Where(pi => pi.IdProperty == request.PropertyId)
                .ToList();

            if (propertyTraceToDelete.Any())
            {
                _context.PropertyTraces.RemoveRange(propertyTraceToDelete);
                _ = await _context.SaveChangesAsync();
            }

            return new OkResult();

        }
    }
}

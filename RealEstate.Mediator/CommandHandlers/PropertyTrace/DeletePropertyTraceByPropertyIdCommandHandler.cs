using MediatR;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Mediator.Commands.PropertyImageCommand;
using RealEstate.Mediator.Commands.PropertyTraceCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var propertyTraceToDelete = _context.PropertyTraces
                .Where(pi => pi.IdProperty == request.PropertyId)
                .ToList();

            if (propertyTraceToDelete.Any())
            {
                _context.PropertyTraces.RemoveRange(propertyTraceToDelete);
                await _context.SaveChangesAsync();
            }

            return new OkResult();

        }
    }
}

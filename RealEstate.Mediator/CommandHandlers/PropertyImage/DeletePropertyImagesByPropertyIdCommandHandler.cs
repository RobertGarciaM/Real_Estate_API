using MediatR;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Mediator.Commands.PropertyImageCommand;

namespace RealEstate.Mediator.CommandHandlers.PropertyImageHandler
{
    internal class DeletePropertyImagesByPropertyIdCommandHandler : IRequestHandler<DeletePropertyImagesByPropertyIdCommand, ActionResult>
    {
        private readonly InMemoryDbContext _context;

        public DeletePropertyImagesByPropertyIdCommandHandler(InMemoryDbContext context)
        {
            _context = context;
        }

        public async Task<ActionResult> Handle(DeletePropertyImagesByPropertyIdCommand request, CancellationToken cancellationToken)
        {
            var propertyImagesToDelete = _context.PropertyImages
                .Where(pi => pi.IdProperty == request.PropertyId)
                .ToList();

            if (!propertyImagesToDelete.Any())
            {
                return new NotFoundResult();
            }
            _context.PropertyImages.RemoveRange(propertyImagesToDelete);
            await _context.SaveChangesAsync();
            return new OkResult();
        }
    }
}
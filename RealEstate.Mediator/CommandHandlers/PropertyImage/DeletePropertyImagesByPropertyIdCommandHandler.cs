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
            List<DataModels.PropertyImage> propertyImagesToDelete = _context.PropertyImages
                .Where(pi => pi.IdProperty == request.PropertyId)
                .ToList();

            if (propertyImagesToDelete.Any())
            {
                _context.PropertyImages.RemoveRange(propertyImagesToDelete);
                _ = await _context.SaveChangesAsync();
            }

            return new OkResult();
        }
    }
}
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Mediator.Commands.PropertyImageCommand;


namespace RealEstate.Mediator.CommandHandlers.PropertyImageHandler
{
    internal class DeletePropertyImageCommandHandler : IRequestHandler<DeletePropertyImageCommand, ActionResult>
    {
        private readonly InMemoryDbContext _context;

        public DeletePropertyImageCommandHandler(InMemoryDbContext context)
        {
            _context = context;
        }

        public async Task<ActionResult> Handle(DeletePropertyImageCommand request, CancellationToken cancellationToken)
        {
            var existingPropertyImage = await _context.PropertyImages.FindAsync(request.PropertyImageId);

            if (existingPropertyImage == null)
            {
                return new NotFoundResult();
            }

            _context.PropertyImages.Remove(existingPropertyImage);
            await _context.SaveChangesAsync();

            return new OkResult();
        }
    }
}

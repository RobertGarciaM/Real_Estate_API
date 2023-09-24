using MediatR;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Mediator.Commands.PropertyImageCommand;


namespace RealEstate.Mediator.CommandHandlers.PropertyImageHandler
{
    internal class DeletePropertyImageCommandHandler : IRequestHandler<DeletePropertyImageCommand, ActionResult>
    {
        private readonly RealEstateDbContext _context;

        public DeletePropertyImageCommandHandler(RealEstateDbContext context)
        {
            _context = context;
        }

        public async Task<ActionResult> Handle(DeletePropertyImageCommand request, CancellationToken cancellationToken)
        {
            DataModels.PropertyImage? existingPropertyImage = await _context.PropertyImages.FindAsync(request.PropertyImageId);

            if (existingPropertyImage == null)
            {
                return new NotFoundResult();
            }

            _ = _context.PropertyImages.Remove(existingPropertyImage);
            _ = await _context.SaveChangesAsync();

            return new OkResult();
        }
    }
}

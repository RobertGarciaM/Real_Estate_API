using RealEstate.Mediator.CommandHandlers.PropertyImageHandler;
using RealEstate.Mediator.Commands.PropertyImageCommand;

namespace RealEstate.Mediator.Test.DeletePropertyImage
{
    public class DeletePropertyImageCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ExistingPropertyImage_ReturnsOkResult()
        {
            // Arrange
            Guid propertyImageId = Guid.NewGuid();

            using InMemoryDbContext context = new();
            PropertyImage existingPropertyImage = new()
            {
                IdPropertyImage = propertyImageId,
                Enabled = true,
                IdProperty = Guid.NewGuid(),
                File = new byte[] { 0x12, 0x34, 0x56 }
            };
            _ = context.PropertyImages.Add(existingPropertyImage);
            _ = context.SaveChanges();

            DeletePropertyImageCommandHandler handler = new(context);
            DeletePropertyImageCommand request = new() { PropertyImageId = propertyImageId };

            // Act
            ActionResult result = await handler.Handle(request, CancellationToken.None);

            // Assert
            _ = Assert.IsType<OkResult>(result);
            PropertyImage? deletedImage = await context.PropertyImages.FindAsync(propertyImageId);
            Assert.Null(deletedImage);
        }

        [Fact]
        public async Task Handle_NonExistentPropertyImage_ReturnsNotFoundResult()
        {
            // Arrange
            Guid propertyImageId = Guid.NewGuid();

            using InMemoryDbContext context = new();
            DeletePropertyImageCommandHandler handler = new(context);
            DeletePropertyImageCommand request = new() { PropertyImageId = propertyImageId };

            // Act
            ActionResult result = await handler.Handle(request, CancellationToken.None);

            // Assert
            _ = Assert.IsType<NotFoundResult>(result);
        }
    }

}

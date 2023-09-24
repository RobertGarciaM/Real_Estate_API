
namespace RealEstate.Mediator.Test.DeletePropertyImage
{
    public class DeletePropertyImagesByPropertyIdCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ExistingPropertyImages_ReturnsOkResult()
        {
            // Arrange
            Guid propertyId = Guid.NewGuid();

            using RealEstateDbContext context = new();
            List<PropertyImage> propertyImagesToDelete = new()
            {
                new PropertyImage { IdPropertyImage = Guid.NewGuid(),
                    Enabled = true,
                    IdProperty = Guid.NewGuid(),
                    File = new byte[] { 0x12, 0x34, 0x56 }},
                new PropertyImage {  IdPropertyImage =  Guid.NewGuid(),
                    Enabled = true,
                    IdProperty = Guid.NewGuid(),
                    File = new byte[] { 0x12, 0x34, 0x56 }},
                new PropertyImage {  IdPropertyImage =  Guid.NewGuid(),
                    Enabled = true,
                    IdProperty = Guid.NewGuid(),
                    File = new byte[] { 0x12, 0x34, 0x56 } }
            };
            context.PropertyImages.AddRange(propertyImagesToDelete);
            _ = context.SaveChanges();

            DeletePropertyImagesByPropertyIdCommandHandler handler = new(context);
            DeletePropertyImagesByPropertyIdCommand request = new() { PropertyId = propertyId };

            // Act
            ActionResult result = await handler.Handle(request, CancellationToken.None);

            // Assert
            _ = Assert.IsType<OkResult>(result);
            List<PropertyImage> remainingImages = await context.PropertyImages
                .Where(pi => pi.IdProperty == propertyId)
                .ToListAsync();
            Assert.Empty(remainingImages);
        }

        [Fact]
        public async Task Handle_NoPropertyImages_ReturnsOkResult()
        {
            // Arrange
            Guid propertyId = Guid.NewGuid();

            using RealEstateDbContext context = new();
            DeletePropertyImagesByPropertyIdCommandHandler handler = new(context);
            DeletePropertyImagesByPropertyIdCommand request = new() { PropertyId = propertyId };

            // Act
            ActionResult result = await handler.Handle(request, CancellationToken.None);
            _ = Assert.IsType<OkResult>(result);
        }
    }

}

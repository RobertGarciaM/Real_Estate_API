namespace RealEstate.Mediator.nUnitTest.PropertyImageMTest
{
    [TestFixture]
    public class DeletePropertyImageCommandHandlerTests
    {
        [Test]
        public async Task Handle_ExistingPropertyImage_ReturnsOkResult()
        {
            // Arrange
            Guid propertyImageId = Guid.NewGuid();

            using RealEstateDbContext context = new RealEstateDbContext();
            PropertyImage existingPropertyImage = new PropertyImage
            {
                IdPropertyImage = propertyImageId,
                Enabled = true,
                IdProperty = Guid.NewGuid(),
                File = new byte[] { 0x12, 0x34, 0x56 }
            };
            context.PropertyImages.Add(existingPropertyImage);
            context.SaveChanges();

            DeletePropertyImageCommandHandler handler = new DeletePropertyImageCommandHandler(context);
            DeletePropertyImageCommand request = new DeletePropertyImageCommand { PropertyImageId = propertyImageId };

            // Act
            ActionResult result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsInstanceOf<OkResult>(result);
            PropertyImage deletedImage = await context.PropertyImages.FindAsync(propertyImageId);
            Assert.IsNull(deletedImage);
        }

        [Test]
        public async Task Handle_NonExistentPropertyImage_ReturnsNotFoundResult()
        {
            // Arrange
            Guid propertyImageId = Guid.NewGuid();

            using RealEstateDbContext context = new RealEstateDbContext();
            DeletePropertyImageCommandHandler handler = new DeletePropertyImageCommandHandler(context);
            DeletePropertyImageCommand request = new DeletePropertyImageCommand { PropertyImageId = propertyImageId };

            // Act
            ActionResult result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }
    }
}

namespace RealEstate.Mediator.nUnitTest.PropertyImageNTest
{
    [TestFixture]
    public class DeletePropertyImagesByPropertyIdCommandHandlerTests
    {
        [Test]
        public async Task Handle_ExistingPropertyImages_ReturnsOkResult()
        {
            // Arrange
            Guid propertyId = Guid.NewGuid();

            using RealEstateDbContext context = new RealEstateDbContext();
            List<PropertyImage> propertyImagesToDelete = new List<PropertyImage>
            {
                new PropertyImage
                {
                    IdPropertyImage = Guid.NewGuid(),
                    Enabled = true,
                    IdProperty = propertyId,
                    File = new byte[] { 0x12, 0x34, 0x56 }
                },
                new PropertyImage
                {
                    IdPropertyImage = Guid.NewGuid(),
                    Enabled = true,
                    IdProperty = propertyId,
                    File = new byte[] { 0x12, 0x34, 0x56 }
                },
                new PropertyImage
                {
                    IdPropertyImage = Guid.NewGuid(),
                    Enabled = true,
                    IdProperty = propertyId,
                    File = new byte[] { 0x12, 0x34, 0x56 }
                }
            };
            context.PropertyImages.AddRange(propertyImagesToDelete);
            context.SaveChanges();

            DeletePropertyImagesByPropertyIdCommandHandler handler = new DeletePropertyImagesByPropertyIdCommandHandler(context);
            DeletePropertyImagesByPropertyIdCommand request = new DeletePropertyImagesByPropertyIdCommand { PropertyId = propertyId };

            // Act
            ActionResult result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsInstanceOf<OkResult>(result);
            List<PropertyImage> remainingImages = await context.PropertyImages
                .Where(pi => pi.IdProperty == propertyId)
                .ToListAsync();
            Assert.IsEmpty(remainingImages);
        }

        [Test]
        public async Task Handle_NoPropertyImages_ReturnsOkResult()
        {
            // Arrange
            Guid propertyId = Guid.NewGuid();

            using RealEstateDbContext context = new RealEstateDbContext();
            DeletePropertyImagesByPropertyIdCommandHandler handler = new DeletePropertyImagesByPropertyIdCommandHandler(context);
            DeletePropertyImagesByPropertyIdCommand request = new DeletePropertyImagesByPropertyIdCommand { PropertyId = propertyId };

            // Act
            ActionResult result = await handler.Handle(request, CancellationToken.None);
            Assert.IsInstanceOf<OkResult>(result);
        }
    }
}

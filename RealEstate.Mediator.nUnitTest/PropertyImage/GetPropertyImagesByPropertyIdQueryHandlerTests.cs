

namespace RealEstate.Mediator.nUnitTest.PropertyImageNTest
{
    [TestFixture]
    public class GetPropertyImagesByPropertyIdQueryHandlerTests
    {
        [Test]
        public async Task Handle_ReturnsPropertyImages()
        {
            // Arrange
            Guid propertyId = Guid.NewGuid();
            GetPropertyImagesByPropertyIdQuery query = new GetPropertyImagesByPropertyIdQuery(propertyId, 1, 10);

            List<PropertyImage> propertyImagesData = new List<PropertyImage>
            {
                new PropertyImage
                {
                    IdPropertyImage = Guid.NewGuid(),
                    IdProperty = propertyId,
                    Enabled = true,
                    File = new byte[] { 0x12, 0x34, 0x56 }
                }
            };

            using RealEstateDbContext context = new RealEstateDbContext();
            await context.PropertyImages.AddRangeAsync(propertyImagesData);
            await context.SaveChangesAsync();

            MapperConfiguration mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperProfile()));
            IMapper mapper = mapperConfig.CreateMapper();

            GetPropertyImagesByPropertyIdQueryHandler handler = new GetPropertyImagesByPropertyIdQueryHandler(context, mapper);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<List<PropertyImageDto>>(result);
        }
    }
}

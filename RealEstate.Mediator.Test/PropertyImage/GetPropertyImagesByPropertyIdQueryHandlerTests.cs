
namespace RealEstate.Mediator.Test.GetPropertyImage
{
    public class GetPropertyImagesByPropertyIdQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ReturnsPropertyImages()
        {
            // Arrange
            Guid propertyId = Guid.NewGuid();
            GetPropertyImagesByPropertyIdQuery query = new(propertyId, 1, 10);

            List<PropertyImage> propertyImagesData = new()
            {
                new PropertyImage
                {
                    IdPropertyImage = Guid.NewGuid(),
                    IdProperty = propertyId,
                    Enabled= true,
                    File = new byte[] { 0x12, 0x34, 0x56 }
                }
            };

            using RealEstateDbContext context = new();
            await context.PropertyImages.AddRangeAsync(propertyImagesData);
            _ = await context.SaveChangesAsync();

            MapperConfiguration mapperConfig = new(cfg => cfg.AddProfile(new AutoMapperProfile()));
            IMapper mapper = mapperConfig.CreateMapper();

            GetPropertyImagesByPropertyIdQueryHandler handler = new(context, mapper);

            // Act
            IEnumerable<PropertyImageDto> result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            _ = Assert.IsAssignableFrom<IEnumerable<PropertyImageDto>>(result);
        }
    }
}

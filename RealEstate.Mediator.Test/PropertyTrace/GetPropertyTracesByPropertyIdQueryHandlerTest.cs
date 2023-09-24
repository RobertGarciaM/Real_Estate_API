
namespace RealEstate.Mediator.Test.GetPropertyTrace
{
    public class GetPropertyTracesByPropertyIdQueryHandlerTest
    {
        [Fact]
        public async Task Handle_ReturnsPropertyTraces()
        {
            // Arrange
            Guid propertyId = Guid.NewGuid();
            GetPropertyTracesByPropertyIdQuery query = new(propertyId, 1, 10);

            List<PropertyTrace> propertyTracesData = new()
            {
                    new PropertyTrace
                    {
                        IdPropertyTrace = Guid.NewGuid(),
                        IdProperty = propertyId,
                        DateSale = DateTime.Now,
                        Name= "Name",
                        Value = 100000,
                        Tax = 99999
                    }
             };

            using RealEstateDbContext context = new();
            await context.PropertyTraces.AddRangeAsync(propertyTracesData);
            _ = await context.SaveChangesAsync();

            MapperConfiguration mapperConfig = new(cfg => cfg.AddProfile(new AutoMapperProfile()));
            IMapper mapper = mapperConfig.CreateMapper();

            GetPropertyTracesByPropertyIdQueryHandler handler = new(context, mapper);


            // Act
            IEnumerable<PropertyTraceDto> result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            _ = Assert.IsAssignableFrom<IEnumerable<PropertyTraceDto>>(result);
        }
    }
}

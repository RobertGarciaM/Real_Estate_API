using RealEstate.Mediator.QueryHandlers.PropertyQuery;

namespace RealEstate.Mediator.Test.GetProperty
{
    public class GetPropertiesQueryHandlerTests
    {

        [Fact]
        public async Task Handle_ReturnsFilteredPropertiesByYearAndInternalCode()
        {
            // Arrange
            var query = new GetPropertiesQuery
            {
                Year = 1994,
                CodeInternal = "ABC789",
            };

            using InMemoryDbContext context = new();
            await context.Properties.AddRangeAsync(GetSampleProperties());
            _ = await context.SaveChangesAsync();

            MapperConfiguration mapperConfig = new(cfg => cfg.AddProfile(new AutoMapperProfile()));
            IMapper mapper = mapperConfig.CreateMapper();

            GetPropertiesQueryHandler handler = new(context, mapper);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            var propertyDtos = result.ToList();
            Assert.Single(propertyDtos);
            Assert.Equal(2020, propertyDtos[0].Year);
            Assert.Equal("ABC789", propertyDtos[0].CodeInternal);
        }


        private IQueryable<Property> GetSampleProperties()
        {
            IQueryable<Property> properties = new[]
            {
                new Property
                {
                    IdProperty = Guid.NewGuid(),
                    Name = "Property 1",
                    Address = "Address 1",
                    Price = 100000,
                    CodeInternal = "ABC123",
                    Year = 2020,
                    IdOwner = Guid.NewGuid()
                },
                new Property
                {
                    IdProperty = Guid.NewGuid(),
                    Name = "Property 2",
                    Address = "Address 2",
                    Price = 150000,
                    CodeInternal = "XYZ456",
                    Year = 2022,
                    IdOwner = Guid.NewGuid()
                },
                new Property
                {
                    IdProperty = Guid.NewGuid(),
                    Name = "Property 3",
                    Address = "Address 3",
                    Price = 900000,
                    CodeInternal = "ABC789",
                    Year = 2020,
                    IdOwner = Guid.NewGuid()
                }
            }.AsQueryable();

            return properties;
        }
    }
}

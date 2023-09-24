namespace RealEstate.Mediator.nUnitTest.PropertyTraceNTest
{
    [TestFixture]
    public class GetPropertyTracesByPropertyIdQueryHandlerTest
    {
        [Test]
        public async Task Handle_ReturnsPropertyTraces()
        {
            // Arrange
            Guid propertyId = Guid.NewGuid();
            GetPropertyTracesByPropertyIdQuery query = new GetPropertyTracesByPropertyIdQuery(propertyId, 1, 10);

            List<PropertyTrace> propertyTracesData = new List<PropertyTrace>
            {
                new PropertyTrace
                {
                    IdPropertyTrace = Guid.NewGuid(),
                    IdProperty = propertyId,
                    DateSale = DateTime.Now,
                    Name = "Name",
                    Value = 100000,
                    Tax = 99999
                }
            };

            using RealEstateDbContext context = new RealEstateDbContext();
            await context.PropertyTraces.AddRangeAsync(propertyTracesData);
            _ = await context.SaveChangesAsync();

            MapperConfiguration mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperProfile()));
            IMapper mapper = mapperConfig.CreateMapper();

            GetPropertyTracesByPropertyIdQueryHandler handler = new GetPropertyTracesByPropertyIdQueryHandler(context, mapper);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<List<PropertyTraceDto>>(result);
        }
    }
}

namespace RealEstate.Mediator.nUnitTest.PropertyNTest
{
    [TestFixture]
    public class CheckPropertyExistsCommandHandlerTests
    {
        [Test]
        public async Task Handle_ExistingProperty_ReturnsTrue()
        {
            // Arrange
            Guid propertyId = Guid.NewGuid();
            IQueryable<Property> properties = new[]
            {
                new Property
                {
                    IdProperty = propertyId,
                    Name = "Property One",
                    Address = "st Property",
                    Price = 23000000,
                    CodeInternal = "IS2245",
                    Year = 2022
                },
                new Property
                {
                    IdProperty = Guid.NewGuid(),
                    Name = "Property One",
                    Address = "st Property",
                    Price = 23000000,
                    CodeInternal = "IS2245",
                    Year = 2022
                }
            }.AsQueryable();

            using RealEstateDbContext context = new();
            await context.Properties.AddRangeAsync(properties);
            _ = await context.SaveChangesAsync();

            CheckPropertyExistsCommandHandler handler = new(context);
            CheckPropertyExistsCommand query = new(propertyId);

            // Act
            bool result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task Handle_NonExistingProperty_ReturnsFalse()
        {
            // Arrange
            Guid propertyId = Guid.NewGuid();
            using RealEstateDbContext context = new();

            CheckPropertyExistsCommandHandler handler = new(context);
            CheckPropertyExistsCommand query = new(propertyId);

            // Act
            bool result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsFalse(result);
        }
    }
}

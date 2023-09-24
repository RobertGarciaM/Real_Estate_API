public class CheckOwnerExistsQueryHandlerTests
{
    [Fact]
    public async Task Handle_ExistingOwner_ReturnsTrue()
    {
        // Arrange
        Guid ownerId = Guid.NewGuid();
        Owner initialOwner = new()
        {
            IdOwner = ownerId,
            Name = "Initial Name",
            Address = "Initial Address",
            Photo = new byte[] { 0x01, 0x02, 0x03 },
            Birthday = DateTime.Parse("1990-01-01"),
        };

        using RealEstateDbContext context = new();
        _ = context.Owners.Add(initialOwner);
        _ = await context.SaveChangesAsync();


        CheckOwnerExistsQueryHandler handler = new(context);
        CheckOwnerExistsQuery query = new(ownerId);

        // Act
        bool result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task Handle_NonExistingOwner_ReturnsFalse()
    {
        // Arrange
        Guid ownerId = Guid.NewGuid();
        using RealEstateDbContext context = new();

        CheckOwnerExistsQueryHandler handler = new(context);
        CheckOwnerExistsQuery query = new(ownerId);

        // Act
        bool result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result);
    }
}
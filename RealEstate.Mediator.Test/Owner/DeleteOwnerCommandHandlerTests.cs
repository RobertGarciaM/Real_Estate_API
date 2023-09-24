namespace RealEstate.Mediator.Test.DeleteOwner
{
    public class DeleteOwnerCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ExistingOwner_ReturnsOkResult()
        {
            // Arrange
            using InMemoryDbContext context = new();
            Owner owner = new()
            {
                IdOwner = Guid.NewGuid(),
                Name = "John",
                Address = "123 Main St",
                Photo = new byte[] { 0x12, 0x34, 0x56 },
                Birthday = DateTime.Parse("1994-03-03")
            };

            _ = context.Owners.Add(owner);
            _ = context.SaveChanges();

            DeleteOwnerCommandHandler handler = new(context);

            DeleteOwnerCommand request = new() { OwnerId = owner.IdOwner };

            // Act
            ActionResult result = await handler.Handle(request, CancellationToken.None);
            Owner? deletedOwner = await context.Owners.FindAsync(owner.IdOwner);
            // Assert
            _ = Assert.IsType<OkResult>(result);
            Assert.Null(deletedOwner);
        }

        [Fact]
        public async Task Handle_NonExistingOwner_ReturnsNotFoundResult()
        {
            // Arrange
            using InMemoryDbContext context = new();
            Owner owner = new()
            {
                IdOwner = Guid.NewGuid(),
                Name = "John",
                Address = "123 Main St",
                Photo = new byte[] { 0x12, 0x34, 0x56 },
                Birthday = DateTime.Parse("1994-03-03")
            };
            _ = context.Owners.Add(owner);
            _ = context.SaveChanges();
            DeleteOwnerCommandHandler handler = new(context);
            Guid ownerId = Guid.NewGuid();

            DeleteOwnerCommand request = new() { OwnerId = ownerId };

            // Act
            ActionResult result = await handler.Handle(request, CancellationToken.None);

            // Assert
            _ = Assert.IsType<NotFoundResult>(result);
        }
    }
}

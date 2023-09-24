
namespace RealEstate.Mediator.nUnitTest.OwnerNTest
{
    [TestFixture]
    public class DeleteOwnerCommandHandlerTests
    {
        [Test]
        public async Task Handle_ExistingOwner_ReturnsOkResult()
        {
            // Arrange
            using RealEstateDbContext context = new RealEstateDbContext();
            Owner owner = new Owner
            {
                IdOwner = Guid.NewGuid(),
                Name = "John",
                Address = "123 Main St",
                Photo = new byte[] { 0x12, 0x34, 0x56 },
                Birthday = DateTime.Parse("1994-03-03")
            };

            context.Owners.Add(owner);
            context.SaveChanges();

            DeleteOwnerCommandHandler handler = new DeleteOwnerCommandHandler(context);

            DeleteOwnerCommand request = new DeleteOwnerCommand { OwnerId = owner.IdOwner };

            // Act
            ActionResult result = await handler.Handle(request, CancellationToken.None);
            Owner deletedOwner = await context.Owners.FindAsync(owner.IdOwner);

            // Assert
            Assert.IsInstanceOf<OkResult>(result);
            Assert.IsNull(deletedOwner);
        }

        [Test]
        public async Task Handle_NonExistingOwner_ReturnsNotFoundResult()
        {
            // Arrange
            using RealEstateDbContext context = new RealEstateDbContext();
            Owner owner = new Owner
            {
                IdOwner = Guid.NewGuid(),
                Name = "John",
                Address = "123 Main St",
                Photo = new byte[] { 0x12, 0x34, 0x56 },
                Birthday = DateTime.Parse("1994-03-03")
            };
            context.Owners.Add(owner);
            context.SaveChanges();
            DeleteOwnerCommandHandler handler = new DeleteOwnerCommandHandler(context);
            Guid ownerId = Guid.NewGuid();

            DeleteOwnerCommand request = new DeleteOwnerCommand { OwnerId = ownerId };

            // Act
            ActionResult result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }
    }
}

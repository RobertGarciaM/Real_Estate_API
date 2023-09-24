using RealEstate.Mediator.Query.Owner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Mediator.nUnitTest.OwnerNTest
{
    [TestFixture]
    public class CheckOwnerExistsQueryHandlerTests
    {
        [Test]
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

            using RealEstateDbContext context = new RealEstateDbContext();

            context.Owners.Add(initialOwner);
            await context.SaveChangesAsync();

            CheckOwnerExistsQueryHandler handler = new CheckOwnerExistsQueryHandler(context);
            CheckOwnerExistsQuery query = new CheckOwnerExistsQuery(ownerId);

            // Act
            bool result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task Handle_NonExistingOwner_ReturnsFalse()
        {
            // Arrange
            Guid ownerId = Guid.NewGuid();
            using RealEstateDbContext context = new RealEstateDbContext();

            CheckOwnerExistsQueryHandler handler = new CheckOwnerExistsQueryHandler(context);
            CheckOwnerExistsQuery query = new CheckOwnerExistsQuery(ownerId);

            // Act
            bool result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsFalse(result);
        }
    }
}

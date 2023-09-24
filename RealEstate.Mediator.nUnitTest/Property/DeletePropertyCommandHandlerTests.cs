using MediatR;
using RealEstate.Mediator.CommandHandlers.PropertyHandler;

namespace RealEstate.Mediator.nUnitTest.PropertyNTest
{
    [TestFixture]
    public class DeletePropertyCommandHandlerTests
    {
        [Test]
        public async Task Handle_ExistingProperty_ReturnsOkResult()
        {
            // Arrange
            Guid propertyId = Guid.NewGuid();
            Mock<IMediator> mediatorMock = new();
            _ = mediatorMock.Setup(mediator => mediator.Send(It.IsAny<DeletePropertyImagesByPropertyIdCommand>(), CancellationToken.None))
                .ReturnsAsync(new OkResult());
            _ = mediatorMock.Setup(mediator => mediator.Send(It.IsAny<DeletePropertyTraceByPropertyIdCommand>(), CancellationToken.None))
                .ReturnsAsync(new OkResult());

            using RealEstateDbContext context = new();
            Property existingProperty = new()
            {
                IdProperty = propertyId,
                IdOwner = Guid.NewGuid(),
                Name = "Property Updated",
                Address = "st Prope Pope",
                Price = 2000000,
                CodeInternal = "IS56H78F",
                Year = 2024
            };
            _ = context.Properties.Add(existingProperty);
            _ = context.SaveChanges();

            DeletePropertyCommandHandler handler = new(context, mediatorMock.Object);
            DeletePropertyCommand request = new() { PropertyId = propertyId };

            // Act
            ActionResult result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.That(result, Is.TypeOf<OkResult>());
            Property deletedProperty = await context.Properties.FindAsync(propertyId);
            Assert.IsNull(deletedProperty);
        }

        [Test]
        public async Task Handle_NonExistentProperty_ReturnsNotFoundResult()
        {
            // Arrange
            Guid propertyId = Guid.NewGuid();
            Mock<IMediator> mediatorMock = new();
            _ = mediatorMock.Setup(mediator => mediator.Send(It.IsAny<DeletePropertyImagesByPropertyIdCommand>(), CancellationToken.None))
                .ReturnsAsync(new OkResult());
            _ = mediatorMock.Setup(mediator => mediator.Send(It.IsAny<DeletePropertyTraceByPropertyIdCommand>(), CancellationToken.None))
                .ReturnsAsync(new OkResult());

            using RealEstateDbContext context = new();
            DeletePropertyCommandHandler handler = new(context, mediatorMock.Object);
            DeletePropertyCommand request = new() { PropertyId = propertyId };

            // Act
            ActionResult result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }
    }
}

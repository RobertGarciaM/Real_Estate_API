using MediatR;
using RealEstate.Mediator.Commands.PropertyCommand;
using RealEstate.Mediator.Handlers.PropertyHandler;

namespace RealEstate.Mediator.nUnitTest.PropertyNTest
{
    [TestFixture]
    public class CreatePropertyCommandHandlerTests
    {
        [Test]
        public async Task Handle_ValidRequest_ReturnsOkResultAndValidatesSavedData()
        {
            // Arrange
            Guid ownerId = Guid.NewGuid();
            bool ownerExists = true;
            CreatePropertyDto propertyDto = new()
            {
                IdOwner = ownerId,
                Name = "Property One",
                Address = "st Property",
                Price = 23000000,
                CodeInternal = "IS2245",
                Year = 2022
            };

            using RealEstateDbContext context = new();
            Mock<IMediator> mediatorMock = new();
            _ = mediatorMock.Setup(mediator => mediator.Send(It.IsAny<CheckOwnerExistsQuery>(), CancellationToken.None))
                            .ReturnsAsync(ownerExists);

            Mock<IMapper> mapperMock = new();
            Guid propertyId = Guid.NewGuid();
            Property property = new()
            {
                IdProperty = propertyId,
                Name = "Property One",
                Address = "st Property",
                Price = 23000000,
                CodeInternal = "IS2245",
                Year = 2022
            };
            _ = mapperMock.Setup(mapper => mapper.Map<Property>(It.IsAny<CreatePropertyDto>()))
                          .Returns(property);

            CreatePropertyCommandHandler handler = new(context, mapperMock.Object, mediatorMock.Object);

            CreatePropertyCommand request = new(propertyDto);

            // Act
            ActionResult result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.That(result, Is.TypeOf<OkObjectResult>());
            Property addedProperty = await context.Properties.FindAsync(propertyId);
            Assert.IsNotNull(addedProperty);
            Assert.That(addedProperty.Name, Is.EqualTo(propertyDto.Name));
            Assert.That(addedProperty.Address, Is.EqualTo(propertyDto.Address));
            Assert.That(addedProperty.Price, Is.EqualTo(propertyDto.Price));
            Assert.That(addedProperty.CodeInternal, Is.EqualTo(propertyDto.CodeInternal));
            Assert.That(addedProperty.Year, Is.EqualTo(propertyDto.Year));
        }

        [Test]
        public async Task Handle_NonExistentOwner_ReturnsNotFoundResult()
        {
            // Arrange
            bool ownerExists = false;
            CreatePropertyDto propertyDto = new()
            {
                Name = "Property One",
                Address = "st Property",
                Price = 23000000,
                CodeInternal = "IS2245",
                Year = 2022
            };

            using RealEstateDbContext context = new();
            Mock<IMediator> mediatorMock = new();
            _ = mediatorMock.Setup(mediator => mediator.Send(It.IsAny<CheckOwnerExistsQuery>(), CancellationToken.None))
                .ReturnsAsync(ownerExists);

            Mock<IMapper> mapperMock = new();

            CreatePropertyCommandHandler handler = new(context, mapperMock.Object, mediatorMock.Object);

            CreatePropertyCommand request = new(propertyDto);

            // Act
            ActionResult result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.That(result, Is.TypeOf<NotFoundObjectResult>());
            NotFoundObjectResult notFoundObjectResult = (NotFoundObjectResult)result;
            string message = (string)notFoundObjectResult.Value.GetType().GetProperty("Message")?.GetValue(notFoundObjectResult.Value, null);
            Assert.That(message, Is.EqualTo("The Owner does not exist."));
        }

        [Test]
        public void Handle_NullProperty_ThrowsEntityNullException()
        {
            // Arrange
            CreatePropertyDto propertyDto = new()
            {
                Name = "Property One",
                Address = "st Property",
                Price = 23000000,
                CodeInternal = "IS2245",
                Year = 2022
            };
            bool ownerExists = true;
            RealEstateDbContext context = new();
            Mock<IMapper> mapperMock = new();
            Mock<IMediator> mediatorMock = new();
            _ = mediatorMock.Setup(mediator => mediator.Send(It.IsAny<CheckOwnerExistsQuery>(), CancellationToken.None))
                .ReturnsAsync(ownerExists);

            _ = mapperMock.Setup(mapper => mapper.Map<Owner>(It.IsAny<CreateOwnerDto>()))
                      .Returns((Owner)null);

            CreatePropertyCommandHandler handler = new(context, mapperMock.Object, mediatorMock.Object);

            CreatePropertyCommand request = new(propertyDto);

            // Act and Assert
            _ = Assert.ThrowsAsync<EntityNullException>(async () => await handler.Handle(request, CancellationToken.None));
        }
    }
}

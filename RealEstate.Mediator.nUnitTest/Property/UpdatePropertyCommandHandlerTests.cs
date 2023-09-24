

using MediatR;

namespace RealEstate.Mediator.nUnitTest.PropertyNTest
{
    [TestFixture]
    public class UpdatePropertyCommandHandlerTests
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
            OkObjectResult okResult = (OkObjectResult)result;
            Property addedProperty = await context.Properties.FindAsync(propertyId);
            Assert.IsNotNull(addedProperty);
            Assert.That(addedProperty.Name, Is.EqualTo(propertyDto.Name));
            Assert.That(addedProperty.Address, Is.EqualTo(propertyDto.Address));
            Assert.That(addedProperty.Price, Is.EqualTo(propertyDto.Price));
            Assert.That(addedProperty.CodeInternal, Is.EqualTo(propertyDto.CodeInternal));
            Assert.That(addedProperty.Year, Is.EqualTo(propertyDto.Year));
        }

        [Test]
        public async Task Handle_ExistingProperty_ValidOwner_ReturnsOkResult()
        {
            // Arrange
            Guid propertyId = Guid.NewGuid();
            Guid ownerId = Guid.NewGuid();
            Guid newOwnerGuid = Guid.NewGuid();
            UpdatePropertyDto propertyDto = new()
            {
                Id = propertyId,
                IdOwner = newOwnerGuid,
                Name = "Property Updated",
                Address = "st Prope Pope",
                Price = 2000000,
                CodeInternal = "IS56H78F",
                Year = 2024
            };

            bool ownerExists = true;
            using RealEstateDbContext context = new();
            Property existingProperty = new()
            {
                IdProperty = propertyId,
                IdOwner = ownerId,
                Name = "Property Two",
                Address = "st Prope",
                Price = 2345555,
                CodeInternal = "IS56H78",
                Year = 2022
            };
            _ = context.Properties.Add(existingProperty);
            _ = context.SaveChanges();

            Mock<IMediator> mediatorMock = new();
            _ = mediatorMock.Setup(mediator => mediator.Send(It.IsAny<CheckOwnerExistsQuery>(), CancellationToken.None))
                .ReturnsAsync(ownerExists);

            Mock<IMapper> mapperMock = new();
            UpdatePropertyCommandHandler handler = new(context, mapperMock.Object, mediatorMock.Object);
            UpdatePropertyCommand request = new UpdatePropertyCommand { UpdateDto = propertyDto };

            // Act
            ActionResult result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.That(result, Is.TypeOf<OkResult>());
        }

        [Test]
        public async Task Handle_NonExistentProperty_ReturnsNotFoundResult()
        {
            // Arrange
            Guid propertyId = Guid.NewGuid();
            Guid ownerId = Guid.NewGuid();
            UpdatePropertyDto propertyDto = new()
            {
                Id = propertyId,
                IdOwner = ownerId,
                Name = "Property Two",
                Address = "st Prope",
                Price = 2345555,
                CodeInternal = "IS56H78",
                Year = 2022
            };

            bool ownerExists = true;

            DbContextOptions<RealEstateDbContext> options = new DbContextOptionsBuilder<RealEstateDbContext>()
                .UseInMemoryDatabase(databaseName: "RealEstateDataBase")
                .Options;

            using RealEstateDbContext context = new();
            Mock<IMediator> mediatorMock = new();
            _ = mediatorMock.Setup(mediator => mediator.Send(It.IsAny<CheckOwnerExistsQuery>(), CancellationToken.None))
                .ReturnsAsync(ownerExists);

            Mock<IMapper> mapperMock = new();

            UpdatePropertyCommandHandler handler = new(context, mapperMock.Object, mediatorMock.Object);

            UpdatePropertyCommand request = new UpdatePropertyCommand { UpdateDto = propertyDto };

            // Act
            ActionResult result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public async Task Handle_ExistingProperty_InvalidOwner_ReturnsNotFoundObjectResult()
        {
            // Arrange
            Guid propertyId = Guid.NewGuid();
            Guid ownerId = Guid.NewGuid();
            UpdatePropertyDto propertyDto = new()
            {
                Id = propertyId,
                IdOwner = ownerId,
                Name = "Property Two",
                Address = "st Prope",
                Price = 2345555,
                CodeInternal = "IS56H78",
                Year = 2022
            };

            bool ownerExists = false;

            using RealEstateDbContext context = new();
            Property existingProperty = new()
            {
                IdProperty = propertyId,
                IdOwner = ownerId,
                Name = "Property Updated",
                Address = "st Prope Hope",
                Price = 2345555,
                CodeInternal = "IS56H78",
                Year = 2022
            };
            _ = context.Properties.Add(existingProperty);
            _ = context.SaveChanges();

            Mock<IMediator> mediatorMock = new();
            _ = mediatorMock.Setup(mediator => mediator.Send(It.IsAny<CheckOwnerExistsQuery>(), CancellationToken.None))
                .ReturnsAsync(ownerExists);

            Mock<IMapper> mapperMock = new();

            UpdatePropertyCommandHandler handler = new(context, mapperMock.Object, mediatorMock.Object);

            UpdatePropertyCommand request = new UpdatePropertyCommand {UpdateDto = propertyDto };

            // Act
            ActionResult result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.That(result, Is.TypeOf<NotFoundObjectResult>());
            NotFoundObjectResult notFoundObjectResult = (NotFoundObjectResult)result;
            string message = (string)notFoundObjectResult.Value.GetType().GetProperty("Message")?.GetValue(notFoundObjectResult.Value, null);
            Assert.That(message, Is.EqualTo("The Owner does not exists."));
        }
    }
}

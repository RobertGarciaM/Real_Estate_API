using MediatR;
using RealEstate.Mediator.CommandHandlers.PropertyHandler;
using RealEstate.Mediator.Commands.PropertyCommand;
using RealEstate.Mediator.Handlers.PropertyHandler;

namespace RealEstate.Mediator.Test.UpdateProperty
{
    public class UpdatePropertyCommandHandlerTests
    {
        [Fact]
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

            using InMemoryDbContext context = new();
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
            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
            Property? addedProperty = await context.Properties.FindAsync(propertyId);
            Assert.NotNull(addedProperty);
            Assert.Equal(propertyDto.Name, addedProperty.Name);
            Assert.Equal(propertyDto.Address, addedProperty.Address);
            Assert.Equal(propertyDto.Price, addedProperty.Price);
            Assert.Equal(propertyDto.CodeInternal, addedProperty.CodeInternal);
            Assert.Equal(propertyDto.Year, addedProperty.Year);
        }

        [Fact]
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
            using InMemoryDbContext context = new();
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
            UpdatePropertyCommand request = new() { UpdateDto = propertyDto };

            // Act
            ActionResult result = await handler.Handle(request, CancellationToken.None);

            // Assert
            _ = Assert.IsType<OkResult>(result);
        }

        [Fact]
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

            DbContextOptions<InMemoryDbContext> options = new DbContextOptionsBuilder<InMemoryDbContext>()
                .UseInMemoryDatabase(databaseName: "RealEstateDataBase")
                .Options;

            using InMemoryDbContext context = new();
            Mock<IMediator> mediatorMock = new();
            _ = mediatorMock.Setup(mediator => mediator.Send(It.IsAny<CheckOwnerExistsQuery>(), CancellationToken.None))
                .ReturnsAsync(ownerExists);

            Mock<IMapper> mapperMock = new();

            UpdatePropertyCommandHandler handler = new(context, mapperMock.Object, mediatorMock.Object);

            UpdatePropertyCommand request = new() { UpdateDto = propertyDto };

            // Act
            ActionResult result = await handler.Handle(request, CancellationToken.None);

            // Assert
            _ = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
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


            using InMemoryDbContext context = new();
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

            UpdatePropertyCommand request = new() { UpdateDto = propertyDto };

            // Act
            ActionResult result = await handler.Handle(request, CancellationToken.None);

            // Assert
            _ = Assert.IsType<NotFoundObjectResult>(result);
            NotFoundObjectResult notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            string? message = notFoundObjectResult.Value?.GetType().GetProperty("Message")?.GetValue(notFoundObjectResult.Value, null) as string;
            Assert.Equal("The Owner does not exists.", message);
        }
    }
}

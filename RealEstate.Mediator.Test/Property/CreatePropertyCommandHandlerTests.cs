
namespace RealEstate.Mediator.Test.CreateProperty
{
    public class CreatePropertyCommandHandlerTests
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
            _ = Assert.IsType<NotFoundObjectResult>(result);
            NotFoundObjectResult notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            string? message = notFoundObjectResult.Value?.GetType().GetProperty("Message")?.GetValue(notFoundObjectResult.Value, null) as string;
            Assert.Equal("The Owner does not exist.", message);
        }

        [Fact]
        public async Task Handle_NullProperty_ThrowsEntityNullException()
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

            // Act y Assert
            _ = await Assert.ThrowsAsync<EntityNullException>(async () => await handler.Handle(request, CancellationToken.None));
        }
    }
}

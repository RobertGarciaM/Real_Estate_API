using MediatR;
using RealEstate.Mediator.CommandHandlers.PropertyImageHandler;

namespace RealEstate.Mediator.Test.UpdatePropertyImage
{
    public class UpdatePropertyImageCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ValidRequest_ReturnsOkResultAndValidatesUpdatedData()
        {
            // Arrange
            using InMemoryDbContext context = new();
            MapperConfiguration mapperConfig = new(cfg => cfg.AddProfile(new AutoMapperProfile()));
            IMapper mapper = mapperConfig.CreateMapper();
            Mock<IFormFile> formFileMock = new();
            _ = formFileMock.Setup(file => file.FileName).Returns("image.jpg");
            _ = formFileMock.Setup(file => file.Length).Returns(1024);
            _ = formFileMock.Setup(file => file.ContentType).Returns("image/jpeg");
            _ = formFileMock.Setup(file => file.OpenReadStream()).Returns(new MemoryStream());

            Guid propertyImageId = Guid.NewGuid();
            PropertyImage initialPropertyImage = new()
            {
                IdPropertyImage = propertyImageId,
                File = new byte[] { 0x01, 0x02, 0x03 },
                Enabled = true,
                IdProperty = Guid.NewGuid()
            };

            _ = context.PropertyImages.Add(initialPropertyImage);
            _ = await context.SaveChangesAsync();

            UpdatePropertyImagesDto updatedPropertyImageDto = new()
            {
                PropertyImageId = propertyImageId,
                File = formFileMock.Object,
                Enabled = false,
                IdProperty = initialPropertyImage.IdProperty
            };

            UpdatePropertyImageCommand updatePropertyImageCommand = new() { dto = updatedPropertyImageDto };

            UpdatePropertyImageCommandHandler handler = new(context, mapper);

            // Act
            ActionResult result = await handler.Handle(updatePropertyImageCommand, CancellationToken.None);

            // Assert
            OkResult okResult = Assert.IsType<OkResult>(result);

            // Validate that the property image was updated in the database
            PropertyImage? updatedPropertyImage = await context.PropertyImages.FindAsync(propertyImageId);
            Assert.NotNull(updatedPropertyImage);

            // Validate each property value
            Assert.Equal(updatedPropertyImageDto.Enabled, updatedPropertyImage.Enabled);
            Assert.Equal(updatedPropertyImageDto.IdProperty, updatedPropertyImage.IdProperty);
        }

        [Fact]
        public async Task Handle_ExistingPropertyImage_ReturnsOkResult()
        {
            // Arrange
            Guid propertyImageId = Guid.NewGuid();
            Mock<IMediator> mediatorMock = new();
            _ = mediatorMock.Setup(mediator => mediator.Send(It.IsAny<CheckPropertyExistsCommand>(), CancellationToken.None))
                .ReturnsAsync(true);
            Mock<IFormFile> formFileMock = new();
            _ = formFileMock.Setup(file => file.FileName).Returns("image.jpg");
            _ = formFileMock.Setup(file => file.Length).Returns(1024);
            _ = formFileMock.Setup(file => file.ContentType).Returns("image/jpeg");
            _ = formFileMock.Setup(file => file.OpenReadStream()).Returns(new MemoryStream());

            using InMemoryDbContext context = new();
            PropertyImage existingPropertyImage = new()
            {
                IdPropertyImage = propertyImageId,
                Enabled = true,
                File = new byte[] { 0x12, 0x34, 0x56 }
            };
            _ = context.PropertyImages.Add(existingPropertyImage);
            _ = context.SaveChanges();

            UpdatePropertyImagesDto updatedPropertyImageDto = new()
            {
                PropertyImageId = propertyImageId,
                Enabled = false,
                File = formFileMock.Object
            };

            Mock<IMapper> mapperMock = new();
            _ = mapperMock.Setup(mapper => mapper.Map(It.IsAny<UpdatePropertyImagesDto>(), It.IsAny<PropertyImage>()))
                .Callback<UpdatePropertyImagesDto, PropertyImage>((dto, image) =>
                {
                    image.IdPropertyImage = dto.PropertyImageId;
                    image.Enabled = true;
                    image.File = new byte[] { 0x12, 0x34, 0x56 };
                });

            UpdatePropertyImageCommandHandler handler = new(context, mapperMock.Object);
            UpdatePropertyImageCommand request = new() { dto = updatedPropertyImageDto };

            // Act
            ActionResult result = await handler.Handle(request, CancellationToken.None);

            // Assert
            _ = Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Handle_NonExistentPropertyImage_ReturnsNotFoundResult()
        {
            // Arrange
            Guid propertyImageId = Guid.NewGuid();
            Mock<IMediator> mediatorMock = new();
            _ = mediatorMock.Setup(mediator => mediator.Send(It.IsAny<CheckPropertyExistsCommand>(), CancellationToken.None))
                .ReturnsAsync(true);
            Mock<IFormFile> formFileMock = new();
            _ = formFileMock.Setup(file => file.FileName).Returns("image.jpg");
            _ = formFileMock.Setup(file => file.Length).Returns(1024);
            _ = formFileMock.Setup(file => file.ContentType).Returns("image/jpeg");
            _ = formFileMock.Setup(file => file.OpenReadStream()).Returns(new MemoryStream());

            using InMemoryDbContext context = new();
            UpdatePropertyImagesDto updatedPropertyImageDto = new()
            {
                PropertyImageId = propertyImageId,
                Enabled = false,
                File = formFileMock.Object
            };

            Mock<IMapper> mapperMock = new();

            UpdatePropertyImageCommandHandler handler = new(context, mapperMock.Object);
            UpdatePropertyImageCommand request = new() { dto = updatedPropertyImageDto };

            // Act
            ActionResult result = await handler.Handle(request, CancellationToken.None);

            // Assert
            _ = Assert.IsType<NotFoundResult>(result);
        }
    }

}

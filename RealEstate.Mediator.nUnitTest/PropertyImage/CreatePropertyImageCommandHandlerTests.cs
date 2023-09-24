﻿
using MediatR;
using RealEstate.Mediator.CommandHandlers.PropertyImageHandler;
using RealEstate.Mediator.Commands.PropertyImage;

namespace RealEstate.Mediator.nUnitTest.PropertyImageNTest
{
    [TestFixture]
    public class CreatePropertyImageCommandHandlerTests
    {
        [Test]
        public async Task Handle_ExistingProperty_ReturnsOkResultAndValidatesSavedData()
        {
            // Arrange
            Guid propertyId = Guid.NewGuid();
            Guid propertyImageId = Guid.NewGuid();
            Mock<IMapper> mapperMock = new();
            Mock<IMediator> mediatorMock = new();
            _ = mediatorMock.Setup(mediator => mediator.Send(It.IsAny<CheckPropertyExistsCommand>(), CancellationToken.None))
                .ReturnsAsync(true);
            Mock<IFormFile> formFileMock = new();
            _ = formFileMock.Setup(file => file.FileName).Returns("image.jpg");
            _ = formFileMock.Setup(file => file.Length).Returns(1024);
            _ = formFileMock.Setup(file => file.ContentType).Returns("image/jpeg");
            _ = formFileMock.Setup(file => file.OpenReadStream()).Returns(new MemoryStream());

            PropertyImage propertyImageMock = new()
            {
                IdPropertyImage = propertyImageId,
                Enabled = true,
                IdProperty = propertyId,
                File = new byte[] { 0x12, 0x34, 0x56 }
            };

            using RealEstateDbContext context = new();
            CreatePropertyImageDto propertyImageDto = new()
            {
                IdProperty = propertyId,
                Enabled = true,
                File = formFileMock.Object,
            };

            _ = mapperMock.Setup(mapper => mapper.Map<PropertyImage>(It.IsAny<CreatePropertyImageDto>()))
                      .Returns(propertyImageMock);

            CreatePropertyImageCommandHandler handler = new(context, mapperMock.Object, mediatorMock.Object);
            CreatePropertyImageCommand request = new(propertyImageDto);

            // Act
            ActionResult result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            PropertyImage? createdImage = await context.PropertyImages.FirstOrDefaultAsync(pi => pi.IdPropertyImage == propertyImageId);
            Assert.NotNull(createdImage);
            Assert.That(createdImage.IdProperty, Is.EqualTo(propertyImageDto.IdProperty));
            Assert.That(createdImage.Enabled, Is.EqualTo(propertyImageDto.Enabled));
        }

        [Test]
        public async Task Handle_NonExistentProperty_ReturnsNotFoundResult()
        {
            // Arrange
            Guid propertyId = Guid.NewGuid();
            Mock<IMapper> mapperMock = new();
            Mock<IMediator> mediatorMock = new();
            _ = mediatorMock.Setup(mediator => mediator.Send(It.IsAny<CheckPropertyExistsCommand>(), CancellationToken.None))
                .ReturnsAsync(false);
            Mock<IFormFile> formFileMock = new();
            _ = formFileMock.Setup(file => file.FileName).Returns("image.jpg");
            _ = formFileMock.Setup(file => file.Length).Returns(1024);
            _ = formFileMock.Setup(file => file.ContentType).Returns("image/jpeg");
            _ = formFileMock.Setup(file => file.OpenReadStream()).Returns(new MemoryStream());
            PropertyImage propertyImageMock = new()
            {
                IdPropertyImage = propertyId,
                Enabled = true,
                IdProperty = Guid.NewGuid(),
                File = new byte[] { 0x12, 0x34, 0x56 }
            };

            using RealEstateDbContext context = new();
            CreatePropertyImageDto propertyImageDto = new()
            {
                IdProperty = propertyId,
                Enabled = true,
                File = formFileMock.Object,
            };

            _ = mapperMock.Setup(mapper => mapper.Map<PropertyImage>(It.IsAny<CreatePropertyImageDto>()))
                    .Returns(propertyImageMock);

            CreatePropertyImageCommandHandler handler = new(context, mapperMock.Object, mediatorMock.Object);
            CreatePropertyImageCommand request = new(propertyImageDto);

            // Act
            ActionResult result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            NotFoundObjectResult notFoundObjectResult = (NotFoundObjectResult)result;
            string message = (string)notFoundObjectResult.Value.GetType().GetProperty("Message")?.GetValue(notFoundObjectResult.Value, null);
            Assert.That(message, Is.EqualTo("The Property does not exists."));
        }
    }
}
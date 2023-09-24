
namespace RealEstate.Mediator.Test.CreateOwner
{
    public class CreateOwnerCommandHandlerTests
    {

        [Fact]
        public async Task Handle_CreateOwner_ReturnsOkResult()
        {
            // Arrange
            using RealEstateDbContext context = new();
            Mock<IMapper> mapperMock = new();
            Mock<IFormFile> formFileMock = new();
            _ = formFileMock.Setup(file => file.FileName).Returns("image.jpg");
            _ = formFileMock.Setup(file => file.Length).Returns(1024);
            _ = formFileMock.Setup(file => file.ContentType).Returns("image/jpeg");
            _ = formFileMock.Setup(file => file.OpenReadStream()).Returns(new MemoryStream());

            CreateOwnerDto createOwnerDtoMock = new()
            {
                Name = "John Doe",
                Address = "123 Main St",
                Photo = formFileMock.Object,
                Birthday = DateTime.Parse("1994-03-03")
            };

            Guid ownerGuid = Guid.NewGuid();
            Owner ownerMock = new()
            {
                IdOwner = ownerGuid,
                Name = "John Doe",
                Address = "123 Main St",
                Photo = new byte[] { 0x12, 0x34, 0x56 },
                Birthday = DateTime.Parse("1994-03-03")
            };

            _ = mapperMock.Setup(mapper => mapper.Map<Owner>(It.IsAny<CreateOwnerDto>()))
                      .Returns(ownerMock);

            CreateOwnerCommandHandler handler = new(context, mapperMock.Object);

            CreateOwnerCommand request = new(createOwnerDtoMock);

            // Act
            ActionResult result = await handler.Handle(request, CancellationToken.None);

            // Assert
            _ = Assert.IsType<OkObjectResult>(result);
            Owner? addedOwner = await context.Owners.FindAsync(ownerGuid);
            Assert.NotNull(addedOwner);
            Assert.Equal(ownerGuid, addedOwner.IdOwner);
            Assert.Equal("John Doe", addedOwner.Name);
            Assert.Equal("123 Main St", addedOwner.Address);
            Assert.Equal(ownerMock.Photo, addedOwner.Photo);
            Assert.Equal(DateTime.Parse("1994-03-03"), addedOwner.Birthday);
        }


        [Fact]
        public async Task Handle_NullOwner_ThrowsEntityNullException()
        {
            // Arrange
            RealEstateDbContext ownerRepositoryMock = new();
            Mock<IMapper> mapperMock = new();

            _ = mapperMock.Setup(mapper => mapper.Map<Owner>(It.IsAny<CreateOwnerDto>()))
                      .Returns((Owner)null);

            CreateOwnerCommandHandler handler = new(ownerRepositoryMock, mapperMock.Object);

            CreateOwnerCommand request = new(new CreateOwnerDto());

            // Act y Assert
            _ = await Assert.ThrowsAsync<EntityNullException>(async () => await handler.Handle(request, CancellationToken.None));
        }
    }
}
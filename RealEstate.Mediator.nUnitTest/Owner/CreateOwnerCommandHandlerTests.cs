namespace RealEstate.Mediator.nUnitTest.OwnerNTest
{
    [TestFixture]
    public class CreateOwnerCommandHandlerTests
    {
        [Test]
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
            Assert.IsInstanceOf<OkObjectResult>(result);
            Owner addedOwner = await context.Owners.FindAsync(ownerGuid);
            Assert.IsNotNull(addedOwner);
            Assert.That(addedOwner.IdOwner, Is.EqualTo(ownerGuid));
            Assert.That(addedOwner.Name, Is.EqualTo("John Doe"));
            Assert.That(addedOwner.Address, Is.EqualTo("123 Main St"));
            CollectionAssert.AreEqual(ownerMock.Photo, addedOwner.Photo);
            Assert.That(addedOwner.Birthday, Is.EqualTo(DateTime.Parse("1994-03-03")));
        }

        [Test]
        public void Handle_NullOwner_ThrowsEntityNullException()
        {
            // Arrange
            RealEstateDbContext ownerRepositoryMock = new();
            Mock<IMapper> mapperMock = new();
            _ = mapperMock.Setup(mapper => mapper.Map<Owner>(It.IsAny<CreateOwnerDto>()))
                      .Returns((Owner)null);

            CreateOwnerCommandHandler handler = new(ownerRepositoryMock, mapperMock.Object);

            CreateOwnerCommand request = new(new CreateOwnerDto());

            // Act and Assert
            _ = Assert.ThrowsAsync<EntityNullException>(async () => await handler.Handle(request, CancellationToken.None));
        }
    }
}

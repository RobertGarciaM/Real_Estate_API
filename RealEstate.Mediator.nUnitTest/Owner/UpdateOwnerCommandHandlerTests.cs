
namespace RealEstate.Mediator.nUnitTest.OwnerNTest
{
    [TestFixture]
    public class UpdateOwnerCommandHandlerTests
    {
        [Test]
        public async Task Handle_UpdateOwner_ReturnsOkResultAndValidatesUpdatedData()
        {
            // Arrange
            Mock<IMapper> mapperMock = new();
            Mock<IFormFile> formFileMock = new();
            _ = formFileMock.Setup(file => file.FileName).Returns("image.jpg");
            _ = formFileMock.Setup(file => file.Length).Returns(1024);
            _ = formFileMock.Setup(file => file.ContentType).Returns("image/jpeg");
            _ = formFileMock.Setup(file => file.OpenReadStream()).Returns(new MemoryStream());

            using RealEstateDbContext context = new();
            MapperConfiguration mapperConfig = new(cfg => cfg.AddProfile(new AutoMapperProfile()));
            IMapper mapper = mapperConfig.CreateMapper();

            Guid ownerId = Guid.NewGuid();
            Owner initialOwner = new()
            {
                IdOwner = ownerId,
                Name = "Initial Name",
                Address = "Initial Address",
                Photo = new byte[] { 0x01, 0x02, 0x03 },
                Birthday = DateTime.Parse("1990-01-01"),
            };

            _ = context.Owners.Add(initialOwner);
            _ = await context.SaveChangesAsync();

            UpdateOwnerDto updatedOwnerDto = new()
            {
                Id = ownerId,
                Name = "Updated Name",
                Address = "Updated Address",
                Photo = formFileMock.Object,
                Birthday = DateTime.Parse("2000-02-02"),
            };

            UpdateOwnerCommand updateOwnerCommand = new() { UpdateDto = updatedOwnerDto };

            UpdateOwnerCommandHandler handler = new(context, mapper);

            // Act
            ActionResult result = await handler.Handle(updateOwnerCommand, CancellationToken.None);

            // Assert
            Assert.IsInstanceOf<OkResult>(result);
            Owner updatedOwner = await context.Owners.FindAsync(ownerId);
            Assert.IsNotNull(updatedOwner);
            Assert.That(updatedOwner.Name, Is.EqualTo(updatedOwnerDto.Name));
            Assert.That(updatedOwner.Address, Is.EqualTo(updatedOwnerDto.Address));
            Assert.That(updatedOwner.Birthday, Is.EqualTo(updatedOwnerDto.Birthday));
        }

        [Test]
        public async Task Handle_ExistingOwner_ReturnsOkResult()
        {
            // Arrange
            DbContextOptions<RealEstateDbContext> options = new DbContextOptionsBuilder<RealEstateDbContext>()
                .UseInMemoryDatabase(databaseName: "RealEstateDataBase")
                .Options;

            Mock<IFormFile> formFileMock = new();
            _ = formFileMock.Setup(file => file.FileName).Returns("image.jpg");
            _ = formFileMock.Setup(file => file.Length).Returns(1024);
            _ = formFileMock.Setup(file => file.ContentType).Returns("image/jpeg");
            _ = formFileMock.Setup(file => file.OpenReadStream()).Returns(new MemoryStream());

            using RealEstateDbContext context = new(options);
            Guid ownerId = Guid.NewGuid();
            Mock<IMapper> mapperMock = new();

            UpdateOwnerDto updateOwnerDto = new()
            {
                Id = ownerId,
                Name = "John F",
                Address = "1234 Main St",
                Photo = formFileMock.Object,
                Birthday = DateTime.Parse("1994-03-03")
            };

            Owner existingOwner = new()
            {
                IdOwner = ownerId,
                Name = "John",
                Address = "123 Main St",
                Photo = new byte[] { 0x12, 0x34, 0x56 },
                Birthday = DateTime.Parse("1994-03-08")
            };

            _ = context.Owners.Add(existingOwner);
            _ = await context.SaveChangesAsync();

            UpdateOwnerCommandHandler handler = new(context, mapperMock.Object);

            UpdateOwnerCommand request = new() { UpdateDto = updateOwnerDto };

            // Act
            ActionResult result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsInstanceOf<OkResult>(result);
            Owner ownerResult = await context.Owners.FirstOrDefaultAsync(x => x.IdOwner == ownerId);
        }

        [Test]
        public async Task Handle_NonExistingOwner_ReturnsNotFoundResult()
        {
            // Arrange
            Guid ownerId = Guid.NewGuid();
            RealEstateDbContext ownerRepositoryMock = new();
            Mock<IMapper> mapperMock = new();

            UpdateOwnerDto updateOwnerDto = new()
            {
                Id = ownerId
            };

            UpdateOwnerCommandHandler handler = new(ownerRepositoryMock, mapperMock.Object);

            UpdateOwnerCommand request = new() { UpdateDto = updateOwnerDto };

            // Act
            ActionResult result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }
    }
}

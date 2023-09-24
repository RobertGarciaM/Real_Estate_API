
namespace RealEstate.Mediator.Test.OwnerHandler
{
    public class UpdateOwnerCommandHandlerTests
    {
        [Fact]
        public async Task Handle_UpdateOwner_ReturnsOkResultAndValidatesUpdatedData()
        {
            // Arrange
            Mock<IMapper> mapperMock = new();
            Mock<IFormFile> formFileMock = new();
            _ = formFileMock.Setup(file => file.FileName).Returns("image.jpg");
            _ = formFileMock.Setup(file => file.Length).Returns(1024);
            _ = formFileMock.Setup(file => file.ContentType).Returns("image/jpeg");
            _ = formFileMock.Setup(file => file.OpenReadStream()).Returns(new MemoryStream());

            using InMemoryDbContext context = new();
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
            OkResult okResult = Assert.IsType<OkResult>(result);
            Owner? updatedOwner = await context.Owners.FindAsync(ownerId);
            Assert.NotNull(updatedOwner);
            Assert.Equal(updatedOwnerDto.Name, updatedOwner.Name);
            Assert.Equal(updatedOwnerDto.Address, updatedOwner.Address);
            Assert.Equal(updatedOwnerDto.Birthday, updatedOwner.Birthday);
        }

        [Fact]
        public async Task Handle_ExistingOwner_ReturnsOkResult()
        {
            // Arrange
            DbContextOptions<InMemoryDbContext> options = new DbContextOptionsBuilder<InMemoryDbContext>()
                .UseInMemoryDatabase(databaseName: "RealEstateDataBase")
                .Options;

            Mock<IFormFile> formFileMock = new();
            _ = formFileMock.Setup(file => file.FileName).Returns("image.jpg");
            _ = formFileMock.Setup(file => file.Length).Returns(1024);
            _ = formFileMock.Setup(file => file.ContentType).Returns("image/jpeg");
            _ = formFileMock.Setup(file => file.OpenReadStream()).Returns(new MemoryStream());


            using InMemoryDbContext context = new();
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
            _ = context.SaveChanges();

            UpdateOwnerCommandHandler handler = new(context, mapperMock.Object);

            UpdateOwnerCommand request = new() { UpdateDto = updateOwnerDto };

            // Act
            ActionResult result = await handler.Handle(request, CancellationToken.None);

            // Assert
            _ = Assert.IsType<OkResult>(result);
            Owner? ownerResult = await context.Owners.FirstOrDefaultAsync(x => x.IdOwner == ownerId);
        }

        [Fact]
        public async Task Handle_NonExistingOwner_ReturnsNotFoundResult()
        {
            // Arrange
            Guid ownerId = Guid.NewGuid();
            InMemoryDbContext ownerRepositoryMock = new();
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
            _ = Assert.IsType<NotFoundResult>(result);
        }
    }
}

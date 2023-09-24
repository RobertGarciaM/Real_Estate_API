using RealEstate.Mediator.CommandHandlers.PropertyTraceHandler;

namespace RealEstate.Mediator.nUnitTest.PropertyTraceNTest
{
    [TestFixture]
    public class CreatePropertyTraceCommandHandlerTests
    {
        [Test]
        public async Task Handle_CreatesPropertyTrace_ReturnsOkResultAndValidatesSavedData()
        {
            // Arrange
            Guid idProperty = Guid.NewGuid();
            Guid idPropertyTrace = Guid.NewGuid();
            CreatePropertyTraceDto propertyTraceDto = new()
            {
                Name = "Name",
                DateSale = DateTime.Parse("1994-03-03"),
                Tax = 9999,
                Value = 999,
                IdProperty = idProperty
            };

            using RealEstateDbContext context = new();
            Mock<IMapper> mapperMock = new();
            _ = mapperMock.Setup(mapper => mapper.Map<PropertyTrace>(It.IsAny<CreatePropertyTraceDto>()))
                      .Returns<CreatePropertyTraceDto>(dto =>
                      {
                          return new PropertyTrace
                          {
                              IdPropertyTrace = idPropertyTrace,
                              Name = "Name",
                              DateSale = DateTime.Parse("1994-03-03"),
                              Tax = 9999,
                              Value = 999,
                              IdProperty = idProperty
                          };
                      });

            CreatePropertyTraceCommandHandler handler = new(context, mapperMock.Object);
            CreatePropertyTraceCommand request = new(propertyTraceDto);

            // Act
            ActionResult result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.That(result, Is.TypeOf<OkObjectResult>());
            PropertyTrace createdPropertyTrace = await context.PropertyTraces.FirstOrDefaultAsync(pt => pt.IdProperty == idProperty);
            Assert.NotNull(createdPropertyTrace);
            Assert.That(createdPropertyTrace.Name, Is.EqualTo(propertyTraceDto.Name));
            Assert.That(createdPropertyTrace.DateSale, Is.EqualTo(propertyTraceDto.DateSale));
            Assert.That(createdPropertyTrace.Tax, Is.EqualTo(propertyTraceDto.Tax));
            Assert.That(createdPropertyTrace.Value, Is.EqualTo(propertyTraceDto.Value));
            Assert.That(createdPropertyTrace.IdProperty, Is.EqualTo(propertyTraceDto.IdProperty));
        }
    }
}

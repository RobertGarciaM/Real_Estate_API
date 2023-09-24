using RealEstate.Mediator.CommandHandlers.PropertyTraceHandler;
using RealEstate.Mediator.Commands.PropertyTraceCommand;

namespace RealEstate.Mediator.Test.CreatePropertyTrace
{
    public class CreatePropertyTraceCommandHandlerTests
    {
        [Fact]
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

            using InMemoryDbContext context = new();
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
            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
            PropertyTrace? createdPropertyTrace = await context.PropertyTraces.FirstOrDefaultAsync(pt => pt.IdProperty == idProperty);
            Assert.NotNull(createdPropertyTrace);
            Assert.Equal(propertyTraceDto.Name, createdPropertyTrace.Name);
            Assert.Equal(propertyTraceDto.DateSale, createdPropertyTrace.DateSale);
            Assert.Equal(propertyTraceDto.Tax, createdPropertyTrace.Tax);
            Assert.Equal(propertyTraceDto.Value, createdPropertyTrace.Value);
            Assert.Equal(propertyTraceDto.IdProperty, createdPropertyTrace.IdProperty);
        }

    }

}

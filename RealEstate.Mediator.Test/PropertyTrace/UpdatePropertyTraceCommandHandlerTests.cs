using RealEstate.Mediator.CommandHandlers.PropertyTraceHandler;
using RealEstate.Mediator.Commands.PropertyTraceCommand;

namespace RealEstate.Mediator.Test.UpdatePropertyTrace
{
    public class UpdatePropertyTraceCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ValidRequest_ReturnsOkResultAndValidatesUpdatedData()
        {
            // Arrange
            using InMemoryDbContext context = new();
            MapperConfiguration mapperConfig = new(cfg => cfg.AddProfile(new AutoMapperProfile()));
            IMapper mapper = mapperConfig.CreateMapper();

            Guid propertyTraceId = Guid.NewGuid();
            PropertyTrace initialPropertyTrace = new()
            {
                IdPropertyTrace = propertyTraceId,
                DateSale = DateTime.Now,
                Name = "Initial Name",
                Value = 100000,
                Tax = 9999,
                IdProperty = Guid.NewGuid()
            };

            _ = context.PropertyTraces.Add(initialPropertyTrace);
            _ = await context.SaveChangesAsync();

            UpdatePropertyTraceDto updatedPropertyTraceDto = new()
            {
                Id = propertyTraceId,
                DateSale = DateTime.Now.AddDays(1),
                Name = "Updated Name",
                Value = 200000,
                Tax = 5555,
                IdProperty = initialPropertyTrace.IdProperty
            };

            UpdatePropertyTraceCommand updatePropertyTraceCommand = new() { dto = updatedPropertyTraceDto };

            UpdatePropertyTraceCommandHandler handler = new(context, mapper);

            // Act
            ActionResult result = await handler.Handle(updatePropertyTraceCommand, CancellationToken.None);

            // Assert
            OkResult okResult = Assert.IsType<OkResult>(result);
            PropertyTrace? updatedPropertyTrace = await context.PropertyTraces.FindAsync(propertyTraceId);
            Assert.NotNull(updatedPropertyTrace);
            Assert.Equal(updatedPropertyTraceDto.DateSale, updatedPropertyTrace.DateSale);
            Assert.Equal(updatedPropertyTraceDto.Name, updatedPropertyTrace.Name);
            Assert.Equal(updatedPropertyTraceDto.Value, updatedPropertyTrace.Value);
            Assert.Equal(updatedPropertyTraceDto.Tax, updatedPropertyTrace.Tax);
            Assert.Equal(updatedPropertyTraceDto.IdProperty, updatedPropertyTrace.IdProperty);
        }

        [Fact]
        public async Task Handle_ExistingPropertyTrace_ReturnsOkResult()
        {
            // Arrange
            Guid propertyTraceId = Guid.NewGuid();
            Guid propertyId = Guid.NewGuid();

            using InMemoryDbContext context = new();
            PropertyTrace existingPropertyTrace = new()
            {
                Name = "Name One",
                DateSale = DateTime.Now,
                Tax = 9999,
                Value = 999,
                IdProperty = propertyId,
                IdPropertyTrace = propertyTraceId
            };
            _ = context.PropertyTraces.Add(existingPropertyTrace);
            _ = context.SaveChanges();

            UpdatePropertyTraceDto updatedPropertyTraceDto = new()
            {
                Id = existingPropertyTrace.IdPropertyTrace,
                Name = "Name Two",
                DateSale = DateTime.Now,
                Tax = 9999,
                Value = 999,
                IdProperty = propertyId
            };

            Mock<IMapper> mapperMock = new();
            _ = mapperMock.Setup(mapper => mapper.Map(It.IsAny<UpdatePropertyTraceDto>(), It.IsAny<PropertyTrace>()))
                .Callback<UpdatePropertyTraceDto, PropertyTrace>((dto, trace) =>
                {
                    trace.IdPropertyTrace = dto.Id;
                    trace.Tax = dto.Tax;
                    trace.Value = dto.Value;
                    trace.Name = dto.Name;
                    trace.DateSale = dto.DateSale;
                });

            UpdatePropertyTraceCommandHandler handler = new(context, mapperMock.Object);
            UpdatePropertyTraceCommand request = new() { dto = updatedPropertyTraceDto };

            // Act
            ActionResult result = await handler.Handle(request, CancellationToken.None);

            // Assert
            _ = Assert.IsType<OkResult>(result);
            PropertyTrace? updatedTrace = await context.PropertyTraces.FindAsync(propertyTraceId);
            Assert.NotNull(updatedTrace);
        }

        [Fact]
        public async Task Handle_NonExistentPropertyTrace_ReturnsNotFoundResult()
        {
            // Arrange
            Guid propertyTraceId = Guid.NewGuid();

            using InMemoryDbContext context = new();
            UpdatePropertyTraceDto updatedPropertyTraceDto = new()
            {
                Id = propertyTraceId,
                Name = "Name Two",
                DateSale = DateTime.Now,
                Tax = 9999,
                Value = 999,
            };

            Mock<IMapper> mapperMock = new();

            UpdatePropertyTraceCommandHandler handler = new(context, mapperMock.Object);
            UpdatePropertyTraceCommand request = new() { dto = updatedPropertyTraceDto };

            // Act
            ActionResult result = await handler.Handle(request, CancellationToken.None);

            // Assert
            _ = Assert.IsType<NotFoundResult>(result);
        }
    }

}

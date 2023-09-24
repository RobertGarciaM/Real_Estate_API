
namespace RealEstate.Mediator.Test.DeletePropertyTrace
{
    public class DeletePropertyTraceCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ExistingPropertyTrace_ReturnsOkResult()
        {
            // Arrange
            Guid propertyTraceId = Guid.NewGuid();
            Guid propertyId = Guid.NewGuid();

            using RealEstateDbContext context = new();
            PropertyTrace existingPropertyTrace = new()
            {
                Name = "Name",
                DateSale = DateTime.Now,
                Tax = 9999,
                Value = 999,
                IdProperty = propertyId,
                IdPropertyTrace = propertyTraceId
            };
            _ = context.PropertyTraces.Add(existingPropertyTrace);
            _ = context.SaveChanges();

            DeletePropertyTraceCommandHandler handler = new(context, null);
            DeletePropertyTraceCommand request = new() { PropertyTraceId = propertyTraceId };

            // Act
            ActionResult result = await handler.Handle(request, CancellationToken.None);

            // Assert
            _ = Assert.IsType<OkResult>(result);
            PropertyTrace? deletedTrace = await context.PropertyTraces.FindAsync(propertyTraceId);
            Assert.Null(deletedTrace);
        }

        [Fact]
        public async Task Handle_NonExistentPropertyTrace_ReturnsNotFoundResult()
        {
            // Arrange
            Guid propertyTraceId = Guid.NewGuid();

            using RealEstateDbContext context = new();
            DeletePropertyTraceCommandHandler handler = new(context, null);
            DeletePropertyTraceCommand request = new() { PropertyTraceId = propertyTraceId };

            // Act
            ActionResult result = await handler.Handle(request, CancellationToken.None);

            // Assert
            _ = Assert.IsType<NotFoundResult>(result);
        }
    }

}

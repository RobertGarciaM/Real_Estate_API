
namespace RealEstate.Mediator.Test.DeletePropertyTrace
{
    public class DeletePropertyTraceByPropertyIdCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ExistingPropertyTraces_ReturnsOkResult()
        {
            // Arrange
            Guid propertyId = Guid.NewGuid();

            using RealEstateDbContext context = new();
            List<PropertyTrace> propertyTracesToDelete = new()
            {
                new PropertyTrace { Name = "Name",
                            DateSale = DateTime.Now,
                            Tax = 9999,
                            Value = 999,
                            IdProperty = propertyId },
                new PropertyTrace { Name = "Name",
                            DateSale = DateTime.Now,
                            Tax = 9999,
                            Value = 999,
                            IdProperty = propertyId},
                new PropertyTrace {  Name = "Name",
                            DateSale = DateTime.Now,
                            Tax = 9999,
                            Value = 999,
                            IdProperty = propertyId }
            };
            context.PropertyTraces.AddRange(propertyTracesToDelete);
            _ = context.SaveChanges();

            DeletePropertyTraceByPropertyIdCommandHandler handler = new(context);
            DeletePropertyTraceByPropertyIdCommand request = new() { PropertyId = propertyId };

            // Act
            ActionResult result = await handler.Handle(request, CancellationToken.None);

            // Assert
            _ = Assert.IsType<OkResult>(result);
            List<PropertyTrace> remainingTraces = await context.PropertyTraces
                .Where(pt => pt.IdProperty == propertyId)
                .ToListAsync();
            Assert.Empty(remainingTraces);
        }

        [Fact]
        public async Task Handle_NoPropertyTraces_ReturnsOkResult()
        {
            // Arrange
            Guid propertyId = Guid.NewGuid();

            using RealEstateDbContext context = new();
            DeletePropertyTraceByPropertyIdCommandHandler handler = new(context);
            DeletePropertyTraceByPropertyIdCommand request = new() { PropertyId = propertyId };

            // Act
            ActionResult result = await handler.Handle(request, CancellationToken.None);

            // Assert
            _ = Assert.IsType<OkResult>(result);
        }
    }

}

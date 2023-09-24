namespace RealEstate.Mediator.nUnitTest.PropertyTraceNTest
{
    [TestFixture]
    public class UpdatePropertyTraceCommandHandlerTests
    {
        [Test]
        public async Task Handle_ValidRequest_ReturnsOkResultAndValidatesUpdatedData()
        {
            // Arrange
            using RealEstateDbContext context = new RealEstateDbContext();
            MapperConfiguration mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperProfile()));
            IMapper mapper = mapperConfig.CreateMapper();

            Guid propertyTraceId = Guid.NewGuid();
            PropertyTrace initialPropertyTrace = new PropertyTrace
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

            UpdatePropertyTraceDto updatedPropertyTraceDto = new UpdatePropertyTraceDto
            {
                Id = propertyTraceId,
                DateSale = DateTime.Now.AddDays(1),
                Name = "Updated Name",
                Value = 200000,
                Tax = 5555,
                IdProperty = initialPropertyTrace.IdProperty
            };

            UpdatePropertyTraceCommand updatePropertyTraceCommand = new UpdatePropertyTraceCommand { dto = updatedPropertyTraceDto };

            UpdatePropertyTraceCommandHandler handler = new UpdatePropertyTraceCommandHandler(context, mapper);

            // Act
            ActionResult result = await handler.Handle(updatePropertyTraceCommand, CancellationToken.None);

            // Assert
            Assert.That(result, Is.TypeOf<OkResult>());
            PropertyTrace? updatedPropertyTrace = await context.PropertyTraces.FindAsync(propertyTraceId);
            Assert.NotNull(updatedPropertyTrace);
            Assert.AreEqual(updatedPropertyTraceDto.DateSale, updatedPropertyTrace.DateSale);
            Assert.AreEqual(updatedPropertyTraceDto.Name, updatedPropertyTrace.Name);
            Assert.AreEqual(updatedPropertyTraceDto.Value, updatedPropertyTrace.Value);
            Assert.AreEqual(updatedPropertyTraceDto.Tax, updatedPropertyTrace.Tax);
            Assert.AreEqual(updatedPropertyTraceDto.IdProperty, updatedPropertyTrace.IdProperty);
        }

        [Test]
        public async Task Handle_ExistingPropertyTrace_ReturnsOkResult()
        {
            // Arrange
            Guid propertyTraceId = Guid.NewGuid();
            Guid propertyId = Guid.NewGuid();

            using RealEstateDbContext context = new RealEstateDbContext();
            PropertyTrace existingPropertyTrace = new PropertyTrace
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

            UpdatePropertyTraceDto updatedPropertyTraceDto = new UpdatePropertyTraceDto
            {
                Id = existingPropertyTrace.IdPropertyTrace,
                Name = "Name Two",
                DateSale = DateTime.Now,
                Tax = 9999,
                Value = 999,
                IdProperty = propertyId
            };

            Mock<IMapper> mapperMock = new Mock<IMapper>();
            _ = mapperMock.Setup(mapper => mapper.Map(updatedPropertyTraceDto, existingPropertyTrace))
                .Callback<UpdatePropertyTraceDto, PropertyTrace>((dto, trace) =>
                {
                    trace.IdPropertyTrace = dto.Id;
                    trace.Tax = dto.Tax;
                    trace.Value = dto.Value;
                    trace.Name = dto.Name;
                    trace.DateSale = dto.DateSale;
                });

            UpdatePropertyTraceCommandHandler handler = new UpdatePropertyTraceCommandHandler(context, mapperMock.Object);
            UpdatePropertyTraceCommand request = new UpdatePropertyTraceCommand { dto = updatedPropertyTraceDto };

            // Act
            ActionResult result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.That(result, Is.TypeOf<OkResult>());
            PropertyTrace? updatedTrace = await context.PropertyTraces.FindAsync(propertyTraceId);
            Assert.NotNull(updatedTrace);
        }

        [Test]
        public async Task Handle_NonExistentPropertyTrace_ReturnsNotFoundResult()
        {
            // Arrange
            Guid propertyTraceId = Guid.NewGuid();

            using RealEstateDbContext context = new RealEstateDbContext();
            UpdatePropertyTraceDto updatedPropertyTraceDto = new UpdatePropertyTraceDto
            {
                Id = propertyTraceId,
                Name = "Name Two",
                DateSale = DateTime.Now,
                Tax = 9999,
                Value = 999,
            };

            Mock<IMapper> mapperMock = new Mock<IMapper>();

            UpdatePropertyTraceCommandHandler handler = new UpdatePropertyTraceCommandHandler(context, mapperMock.Object);
            UpdatePropertyTraceCommand request = new UpdatePropertyTraceCommand { dto = updatedPropertyTraceDto };

            // Act
            ActionResult result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }
    }
}

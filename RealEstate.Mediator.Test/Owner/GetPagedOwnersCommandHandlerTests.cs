namespace RealEstate.Mediator.Test.GetPageOwner
{
    public class GetPagedOwnersCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ReturnsPagedOwners()
        {
            // Arrange
            int pageSize = 5;
            int page = 1;
            List<Owner> owners = Enumerable.Range(1, 10).Select(i => new Owner
            {
                IdOwner = Guid.NewGuid(),
                Name = $"Owner {i}",
                Address = $"Address {i}",
                Birthday = DateTime.Now.AddDays(-i),
            }).ToList();

            GetPagedOwnersQuery query = new(page, pageSize);

            using InMemoryDbContext context = new();
            await context.Owners.AddRangeAsync(owners);
            _ = await context.SaveChangesAsync();

            MapperConfiguration mapperConfig = new(cfg => cfg.AddProfile(new AutoMapperProfile()));
            IMapper mapper = mapperConfig.CreateMapper();

            GetPagedOwnersCommandHandler handler = new(context, mapper);

            // Act
            IEnumerable<OwnerDto> result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            List<OwnerDto> ownerDtos = result.ToList();
            Assert.Equal(pageSize, ownerDtos.Count);

            for (int i = 0; i < pageSize; i++)
            {
                Owner expectedOwner = owners[i];
                OwnerDto actualOwnerDto = ownerDtos[i];

                Assert.Equal(expectedOwner.IdOwner, actualOwnerDto.Id);
                Assert.Equal(expectedOwner.Name, actualOwnerDto.Name);
                Assert.Equal(expectedOwner.Address, actualOwnerDto.Address);
                Assert.Equal(expectedOwner.Birthday, actualOwnerDto.Birthday);
            }
        }
    }
}

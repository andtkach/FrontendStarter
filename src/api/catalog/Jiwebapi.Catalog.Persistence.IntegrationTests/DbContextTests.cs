using Jiwebapi.Catalog.Application.Contracts;
using Jiwebapi.Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;
using Shouldly;

namespace Jiwebapi.Catalog.Persistence.IntegrationTests
{
    public class DbContextTests
    {
        private readonly CatalogDbContext _dbContext;
        private readonly Mock<ILoggedInUserService> _loggedInUserServiceMock;
        private readonly string _loggedInUserId;

        public DbContextTests()
        {
            var dbContextOptions = new DbContextOptionsBuilder<CatalogDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            _loggedInUserId = "00000000-0000-0000-0000-000000000000";
            _loggedInUserServiceMock = new Mock<ILoggedInUserService>();
            _loggedInUserServiceMock.Setup(m => m.UserId).Returns(_loggedInUserId);

            _dbContext = new CatalogDbContext(dbContextOptions, _loggedInUserServiceMock.Object);
        }

        [Fact]
        public async void Save_SetCreatedByProperty()
        {
            var ev = new Event() {EventId = Guid.NewGuid(), Name = "Test event" };

            _dbContext.Events.Add(ev);
            await _dbContext.SaveChangesAsync();

            ev.CreatedBy.ShouldBe(_loggedInUserId);
        }
    }
}

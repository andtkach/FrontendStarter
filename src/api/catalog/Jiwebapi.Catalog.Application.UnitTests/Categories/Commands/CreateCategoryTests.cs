using AutoMapper;
using Jiwebapi.Catalog.Application.Contracts;
using Jiwebapi.Catalog.Application.Contracts.Message;
using Jiwebapi.Catalog.Application.Contracts.Persistence;
using Jiwebapi.Catalog.Application.Features.Categories.Commands.CreateCateogry;
using Jiwebapi.Catalog.Application.Profiles;
using Jiwebapi.Catalog.Application.UnitTests.Mocks;
using Jiwebapi.Catalog.Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace Jiwebapi.Catalog.Application.UnitTests.Categories.Commands
{
    public class CreateCategoryTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IAsyncRepository<Category>> _mockCategoryRepository;
        private readonly Mock<IPublishService> _mockPublishService;
        private readonly Mock<ILogger<CreateCategoryCommandHandler>> _loggerService;
        private readonly Mock<ILoggedInUserService> _loggedInUserServiceMock;
        private readonly string _loggedInUserId;

        public CreateCategoryTests()
        {
            _mockCategoryRepository = RepositoryMocks.GetCategoryRepository();
            _mockPublishService = ServiceMocks.GetPublishService();
            _loggerService = ServiceMocks.GetLoggerService();
            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = configurationProvider.CreateMapper();

            _loggedInUserId = "00000000-0000-0000-0000-000000000000";
            _loggedInUserServiceMock = new Mock<ILoggedInUserService>();
            _loggedInUserServiceMock.Setup(m => m.UserId).Returns(_loggedInUserId);
        }

        [Fact]
        public async Task Handle_ValidCategory_AddedToCategoriesRepo()
        {
            var handler = new CreateCategoryCommandHandler(_mapper, 
                _mockCategoryRepository.Object, 
                _loggedInUserServiceMock.Object, 
                _mockPublishService.Object, 
                _loggerService.Object);

            await handler.Handle(new CreateCategoryCommand() { Name = "Test" }, CancellationToken.None);

            var allCategories = await _mockCategoryRepository.Object.ListAllAsync();
            allCategories.Count.ShouldBe(5);
        }
    }
}

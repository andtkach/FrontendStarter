using Jiwebapi.Catalog.Application.Contracts.Message;
using Jiwebapi.Catalog.Application.Contracts.Persistence;
using Jiwebapi.Catalog.Application.Features.Categories.Commands.CreateCateogry;
using Jiwebapi.Catalog.Domain.Entities;
using Jiwebapi.Catalog.Domain.Message;
using Microsoft.Extensions.Logging;
using Moq;

namespace Jiwebapi.Catalog.Application.UnitTests.Mocks
{
    public static class ServiceMocks
    {
        public static Mock<IPublishService> GetPublishService()
        {
            var mockPublishService = new Mock<IPublishService>();
            mockPublishService.Setup(svc => svc.PublishEvent(new BaseEvent())).ReturnsAsync(true);

            return mockPublishService;
        }

        public static Mock<ILogger<CreateCategoryCommandHandler>> GetLoggerService()
        {
            var mockLoggerService = new Mock<ILogger<CreateCategoryCommandHandler>>();
            return mockLoggerService;
        }
    }
}

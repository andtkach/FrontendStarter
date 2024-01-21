using Jiwebapi.Catalog.Domain.Entities;
using Jiwebapi.Catalog.Domain.Message;

namespace Jiwebapi.Catalog.Application.Contracts.Message
{
    public interface IPublishService
    {
        Task<bool> PublishEvent<T>(T eventData) where T : BaseEvent;
    }
}

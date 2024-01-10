namespace Jiwebapi.Catalog.Application.Contracts.Cache
{
    public interface IHistoryPublisher
    {
        string ChannelName { get; }
        Task Publish(string message);
    }
}
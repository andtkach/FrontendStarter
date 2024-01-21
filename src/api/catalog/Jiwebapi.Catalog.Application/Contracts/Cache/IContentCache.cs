namespace Jiwebapi.Catalog.Application.Contracts.Cache
{
    public interface IContentCache
    {
        int ContentCacheSeconds { get; set; }
        Task Add<T>(string key, T value, int minutes = 10);
        Task<T> Get<T>(string key) where T : class;
        Task Remove(string key);
    }
}
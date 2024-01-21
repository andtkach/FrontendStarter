using Jiwebapi.Catalog.Domain.Common;

namespace Jiwebapi.Catalog.Application.Contracts.Cache;

public interface ISimpleStorageService
{
    Task<string> Set(string value);
    Task<StorageItem?> Get(string id);
    Task Remove(string id);
}
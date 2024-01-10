using Jiwebapi.Catalog.Domain.Common;

namespace Jiwebapi.Catalog.Application.Contracts.Cache;

public interface IObjectStorageService
{
    Task<StorageObject> CreateItem(StorageObject item);
    Task<StorageObject?> GetItemById(string id);
    Task<IEnumerable<StorageObject?>?> GetAllItems();
    Task<StorageObject?> UpdateItem(StorageObject item);
    Task DeleteItem(string id);
}
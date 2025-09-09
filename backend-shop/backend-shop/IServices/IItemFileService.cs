using backend_shop.DTO;
using backend_shop.Entities;
using RFService.IServices;
using RFService.Repo;

namespace backend_shop.IServices
{
    public interface IItemFileService
        : IService<ItemFile>,
            IServiceId<ItemFile>,
            IServiceUuid<ItemFile>,
            IServiceIdUuid<ItemFile>,
            IServiceCreatedAt<ItemFile>,
            IServiceName<ItemFile>,
            IServiceIdUuidName<ItemFile>
    {
        Task<IEnumerable<ItemFile>> AddForItemUuidAsync(Guid itemUuid, FilesCollectionDTO files);

        Task<IEnumerable<ItemFile>> AddForItemIdAsync(Int64 itemId, FilesCollectionDTO files);

        Task<IEnumerable<ItemFile>> GetListForItemIdAsync(Int64 itemId);

        Task<int> GetCountForOwnerIdAsync(Int64 ownerId, QueryOptions? options = null);

        Task<int> GetCountForCurrentUserAsync(QueryOptions? options = null);

        Task<Int64> GetAggregatedSizeForOwnerIdAsync(Int64 ownerId, QueryOptions? options = null);

        Task<Int64> GetAggregatedSizeForCurrentUserAsync(QueryOptions? options = null);
    }
}

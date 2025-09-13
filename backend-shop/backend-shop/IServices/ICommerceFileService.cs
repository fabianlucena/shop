using backend_shop.DTO;
using backend_shop.Entities;
using RFService.IServices;
using RFService.Repo;

namespace backend_shop.IServices
{
    public interface ICommerceFileService
        : IService<CommerceFile>,
            IServiceId<CommerceFile>,
            IServiceUuid<CommerceFile>,
            IServiceIdUuid<CommerceFile>,
            IServiceCreatedAt<CommerceFile>,
            IServiceName<CommerceFile>,
            IServiceIdUuidName<CommerceFile>
    {
        Task<IEnumerable<CommerceFile>> AddForCommerceUuidAsync(Guid commerceUuid, FilesCollectionDTO files);

        Task<IEnumerable<CommerceFile>> AddForCommerceIdAsync(Int64 commerceId, FilesCollectionDTO files);

        Task<IEnumerable<CommerceFile>> GetListForCommerceIdAsync(Int64 commerceId);

        Task<int> GetCountForOwnerIdAsync(Int64 ownerId, QueryOptions? options = null);

        Task<int> GetCountForCurrentUserAsync(QueryOptions? options = null);

        Task<Int64> GetAggregatedSizeForOwnerIdAsync(Int64 ownerId, QueryOptions? options = null);

        Task<Int64> GetAggregatedSizeForCurrentUserAsync(QueryOptions? options = null);
    }
}

using backend_shop.DTO;
using backend_shop.Entities;
using RFService.IServices;

namespace backend_shop.IServices
{
    public interface IItemFileService
        : IService<ItemFile>,
            IServiceId<ItemFile>,
            IServiceUuid<ItemFile>,
            IServiceIdUuid<ItemFile>,
            IServiceSoftDelete<ItemFile>,
            IServiceSoftDeleteCreatedAt<ItemFile>,
            IServiceSoftDeleteCreatedAtUuid<ItemFile>,
            IServiceSoftDeleteId<ItemFile>,
            IServiceSoftDeleteUuid<ItemFile>,
            IServiceName<ItemFile>,
            IServiceIdUuidName<ItemFile>
    {
        Task<int> UpdateForItemUuidAsync(Guid itemUuid, FilesCollectionDTO files);

        Task<int> UpdateForItemIdAsync(Int64 itemId, FilesCollectionDTO files);

        Task<IEnumerable<ItemFile>> GetListForItemIdAsync(Int64 itemId);
    }
}

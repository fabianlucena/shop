using backend_shop.DTO;
using backend_shop.Entities;
using backend_shop.IServices;
using RFService.IRepo;
using RFService.Repo;
using RFService.Services;

namespace backend_shop.Service
{
    public class ItemFileService(
        IRepo<ItemFile> repo,
        IItemService itemService
    )
        : ServiceSoftDeleteCreatedAtIdUuidName<IRepo<ItemFile>, ItemFile>(repo),
            IItemFileService
    {
        public async Task<int> UpdateForItemUuidAsync(Guid itemUuid, FilesCollectionDTO files)
            => await UpdateForItemIdAsync(await itemService.GetSingleIdForUuidAsync(itemUuid), files);

        public async Task<int> UpdateForItemIdAsync(Int64 itemId, FilesCollectionDTO files)
        {
            foreach (var file in files)
            {
                if (file.Content.Length == 0)
                    continue;

                var itemImage = new ItemFile
                {
                    ItemId = itemId,
                    Name = file.Name,
                    ContentType = file.ContentType,
                    Content = file.Content,
                };

                await CreateAsync(itemImage);
            }

            return 0;
        }

        public async Task<IEnumerable<ItemFile>> GetListForItemIdAsync(Int64 itemId)
            => await GetListAsync(new QueryOptions
            {
                Filters = { { "ItemId", itemId } }
            });
    }
}
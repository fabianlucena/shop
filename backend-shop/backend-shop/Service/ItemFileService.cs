using backend_shop.DTO;
using backend_shop.Entities;
using backend_shop.IServices;
using RFService.IRepo;
using RFService.Repo;
using RFService.Services;
using System.Collections.Generic;

namespace backend_shop.Service
{
    public class ItemFileService(
        IRepo<ItemFile> repo,
        IItemService itemService
    )
        : ServiceSoftDeleteCreatedAtIdUuidName<IRepo<ItemFile>, ItemFile>(repo),
            IItemFileService
    {
        public async Task<IEnumerable<ItemFile>> AddForItemUuidAsync(Guid itemUuid, FilesCollectionDTO files)
            => await AddForItemIdAsync(await itemService.GetSingleIdForUuidAsync(itemUuid), files);

        public async Task<IEnumerable<ItemFile>> AddForItemIdAsync(Int64 itemId, FilesCollectionDTO files)
        {
            var result = new List<ItemFile>();
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

                result.Add(await CreateAsync(itemImage));
            }

            return result;
        }

        public async Task<IEnumerable<ItemFile>> GetListForItemIdAsync(Int64 itemId)
            => await GetListAsync(new QueryOptions
            {
                Filters = { { "ItemId", itemId } }
            });
    }
}
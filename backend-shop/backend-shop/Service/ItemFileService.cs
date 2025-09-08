using backend_shop.DTO;
using backend_shop.Entities;
using backend_shop.Exceptions;
using backend_shop.IServices;
using RFOperators;
using RFService.IRepo;
using RFService.Repo;
using RFService.Services;

namespace backend_shop.Service
{
    public class ItemFileService(
        IRepo<ItemFile> repo,
        IItemService itemService,
        IServiceProvider serviceProvider
    )
        : ServiceCreatedAtIdUuidName<IRepo<ItemFile>, ItemFile>(repo),
            IItemFileService
    {
        public override async Task<ItemFile> ValidateForCreationAsync(ItemFile data)
        {
            data = await base.ValidateForCreationAsync(data);

            if (string.IsNullOrWhiteSpace(data.Name))
                throw new NoNameException();

            if (data.Item == null)
            {
                if (data.ItemId <= 0)
                    throw new NoItemException();

                data.Item = await itemService.GetSingleOrDefaultForIdAsync(data.ItemId);
                if (data.Item == null)
                    throw new NoItemException();
            }

            if (data.Item.Store == null)
            {
                if (data.Item.StoreId <= 0)
                    throw new NoStoreException();

                var storeService = serviceProvider.GetRequiredService<IStoreService>();
                data.Item.Store = await storeService.GetSingleOrDefaultForIdAsync(
                    data.Item.StoreId,
                    new QueryOptions
                    {
                        Join = { { "Commerce" } },
                        Switches = { { "IncludeDisabled", true } },
                    }
                );

                if (data.Item.Store == null)
                    throw new NoStoreException();
            }

            var store = data.Item.Store;
            if (store.Commerce == null)
                throw new CommerceDoesNotExistException();

            var userPlanService = serviceProvider.GetRequiredService<IUserPlanService>();

            var totalCount = await GetCountForCurrentUserAsync(new QueryOptions { Switches = { { "IncludeDisabled", true} } });
            var totalCountMax =  await userPlanService.GetMaxTotalItemsImagesForCurrentUser();
            if (totalCount >= totalCountMax)
                throw new TotalItemsImagesLimitReachedException();

            var enabledCount = await GetCountForCurrentUserAsync();
            var enabledCountMax = await userPlanService.GetMaxEnabledItemsImagesForCurrentUser();
            if (enabledCount > enabledCountMax)
                throw new MaxEnabledItemsImagesLimitReachedException();

            var aggregattedSize = await GetAggregatedSizeForCurrentUserAsync(new QueryOptions { Switches = { { "IncludeDisabled", true } } });
            aggregattedSize += data.Content.Length;
            var aggregattedSizeMax = await userPlanService.GetMaxAggregattedSizeItemsImagesForCurrentUser();
            if (aggregattedSize >= aggregattedSizeMax)
                throw new TotalAggregattedSizeItemImagesLimitReachedException();

            var enabledAggregattedSize = await GetAggregatedSizeForCurrentUserAsync();
            enabledAggregattedSize += data.Content.Length;
            var enabledAggregattedSizeMax = await userPlanService.GetMaxEnabledAggregattedSizeItemsImagesForCurrentUser();
            if (enabledAggregattedSize >= enabledAggregattedSizeMax)
                throw new MaxEnabledAggregattedSizeItemImagesLimitReachedException();

            return data;
        }

        public async Task<QueryOptions> GetFilterForCurrentUserAsync(QueryOptions? options = null)
        {
            var itemsId = await itemService.GetListIdForCurrentUserAsync(options);

            options = (options != null) ?
                new QueryOptions(options) :
                new();
            options.AddFilter("ItemId", itemsId);

            return options;
        }

        public async Task<Int64> GetCountForCurrentUserAsync(QueryOptions? options = null)
            => await GetCountAsync(await GetFilterForCurrentUserAsync(options));

        public async Task<Int64> GetAggregatedSizeForCurrentUserAsync(QueryOptions? options = null)
        {
            options = await GetFilterForCurrentUserAsync(options);
            options.Select ??= [Op.Sum(Op.DataLength("Content"))];
            
            return await GetInt64Async(options);
        }

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
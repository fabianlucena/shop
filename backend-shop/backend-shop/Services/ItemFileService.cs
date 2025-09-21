using backend_shop.DTO;
using backend_shop.Entities;
using backend_shop.Exceptions;
using backend_shop.IServices;
using RFAuth.Exceptions;
using RFOperators;
using RFService.IRepo;
using RFService.Repo;
using RFService.Services;

namespace backend_shop.Services
{
    public class ItemFileService(
        IRepo<ItemFile> repo,
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

                var itemService = serviceProvider.GetRequiredService<IItemService>();
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
            var limits = await userPlanService.GetLimitsForCurrentUserAsync();

            if (data.Content.Length > limits[PlanLimitName.MaxItemImageSize])
                throw new ImageIsTooLargeException();

            var itemImagesCount = await GetCountAsync(new QueryOptions {
                Switches = { { "IncludeDisabled", true } },
                Filters = { { "ItemId", data.ItemId } }
            });
            if (itemImagesCount >= limits[PlanLimitName.MaxTotalImagesPerSingleItem])
                throw new TotalImagesPerItemLimitReachedException();

            var totalCount = await GetCountForCurrentUserAsync(new QueryOptions { Switches = { { "IncludeDisabled", true} } });
            if (totalCount >= limits[PlanLimitName.MaxTotalItemsImages])
                throw new TotalItemsImagesLimitReachedException();

            var enabledCount = await GetCountForCurrentUserAsync();
            if (enabledCount > limits[PlanLimitName.MaxEnabledItemsImages])
                throw new MaxEnabledItemsImagesLimitReachedException();

            var aggregatedSize = await GetAggregatedSizeForCurrentUserAsync(new QueryOptions { Switches = { { "IncludeDisabled", true } } });
            aggregatedSize += data.Content.Length;
            if (aggregatedSize >= limits[PlanLimitName.MaxItemsImagesAggregatedSize])
                throw new TotalItemsImagesAggregatedSizeLimitReachedException();

            var enabledAggregatedSize = await GetAggregatedSizeForCurrentUserAsync();
            enabledAggregatedSize += data.Content.Length;
            if (enabledAggregatedSize >= limits[PlanLimitName.MaxEnabledItemsImagesAggregatedSize])
                throw new MaxEnabledItemsImagesAggregatedSizeLimitReachedException();

            return data;
        }

        public async Task<QueryOptions> GetFilterForOwnerIdAsync(Int64 ownerId, QueryOptions? options = null)
        {
            var itemService = serviceProvider.GetRequiredService<IItemService>();
            var itemsId = await itemService.GetListIdForOwnerIdAsync(ownerId, options);

            options = new QueryOptions();
            options.AddFilter("ItemId", itemsId);

            return options;
        }

        public async Task<int> GetCountForOwnerIdAsync(Int64 ownerId, QueryOptions? options = null)
            => await GetCountAsync(await GetFilterForOwnerIdAsync(ownerId, options));

        public Int64 GetCurrentUserId()
        {
            var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
            var httpContext = httpContextAccessor.HttpContext
                ?? throw new NoAuthorizationHeaderException();

            var userId = (httpContext.Items["UserId"] as Int64?)
                ?? throw new NoSessionUserDataException();

            if (userId <= 0)
                throw new NoSessionUserDataException();

            return userId!;
        }

        public async Task<int> GetCountForCurrentUserAsync(QueryOptions? options = null)
            => await GetCountForOwnerIdAsync(GetCurrentUserId(), options);

        public async Task<Int64> GetAggregatedSizeForOwnerIdAsync(Int64 ownerId, QueryOptions? options = null)
        {
            options = await GetFilterForOwnerIdAsync(ownerId, options);
            options.Select ??= [Op.Sum(Op.DataLength("Content"))];

            return await GetInt64Async(options) ?? 0;
        }

        public async Task<Int64> GetAggregatedSizeForCurrentUserAsync(QueryOptions? options = null)
            => await GetAggregatedSizeForOwnerIdAsync(GetCurrentUserId(), options);

        public async Task<IEnumerable<ItemFile>> AddForItemUuidAsync(Guid itemUuid, FilesCollectionDTO files)
        {
            var itemService = serviceProvider.GetRequiredService<IItemService>();
            return await AddForItemIdAsync(await itemService.GetSingleIdForUuidAsync(itemUuid), files);
        }

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
using RFService.Services;
using RFService.IRepo;
using backend_shop.Entities;
using backend_shop.IServices;
using backend_shop.Exceptions;
using RFService.ILibs;
using RFService.Repo;

namespace backend_shop.Service
{
    public class ItemService(
        IRepo<Item> repo,
        IStoreService storeService,
        IBusinessService businessService,
        IUserPlanService userPlanService
    )
        : ServiceSoftDeleteTimestampsIdUuidEnabledName<IRepo<Item>, Item>(repo),
            IItemService
    {
        public override async Task<Item> ValidateForCreationAsync(Item data)
        {
            data = await base.ValidateForCreationAsync(data);

            if (string.IsNullOrWhiteSpace(data.Name))
                throw new NoNameException();

            if (data.StoreId <= 0)
            {
                data.StoreId = data.Store?.Id ?? 0;
                if (data.StoreId <= 0)
                    throw new NoStoreException();
            }

            var store = await storeService.GetSingleOrDefaultForIdAsync(data.StoreId)
                ?? throw new StoreDoesNotExistException();

            _ = await businessService.GetSingleOrDefaultForIdAsync(store.BusinessId)
                ?? throw new BusinessDoesNotExistException();

            var totalStoresCount = await GetCountForCurrentUserAsync(new GetOptions { Filters = { { "IsEnabled", null } } });
            if (totalStoresCount >= (await userPlanService.GetMaxTotalItemsForCurrentUser()))
                throw new TotalItemsLimitReachedException();

            var enabledItemsCount = await GetCountForCurrentUserAsync();
            var enabledItemsMax = await userPlanService.GetMaxEnabledItemsForCurrentUser();
            if (data.IsEnabled && enabledItemsCount >= enabledItemsMax
                || enabledItemsCount > enabledItemsMax
            )
            {
                throw new MaxEnabledItemsLimitReachedException();
            }

            return data;
        }

        public override async Task<IDataDictionary> ValidateForUpdateAsync(IDataDictionary data, GetOptions options)
        {
            data = await base.ValidateForUpdateAsync(data, options);

            if (data.TryGetValue("IsEnabled", out var isEnabledValue)
                && isEnabledValue is bool isEnabled && isEnabled)
            {
                var getOptions = new GetOptions(options);
                getOptions.Filters["IsEnabled"] = null;
                _ = await GetSingleOrDefaultAsync(getOptions)
                    ?? throw new ItemDoesNotExistException();

                var enabledItemsCount = await GetCountForCurrentUserAsync();
                var enabledItemsMax = await userPlanService.GetMaxEnabledItemsForCurrentUser();
                if (enabledItemsCount >= enabledItemsMax)
                    throw new MaxEnabledItemsLimitReachedException();
            }

            return data;
        }

        public async Task<bool> CheckForUuidAndCurrentUserAsync(Guid uuid, GetOptions? options = null)
        {
            var storesId = await storeService.GetListIdForCurrentUserAsync(options);

            options ??= new ();
            options.Filters["IsEnabled"] = null;
            options.Filters["Uuid"] = uuid;
            options.Filters["StoreId"] = storesId;
            _ = await GetSingleOrDefaultAsync(options)
                ?? throw new ItemDoesNotExistException();

            return true;
        }

        public async Task<GetOptions> GetFilterForCurrentUserAsync(GetOptions? options = null)
        {
            var storesId = await storeService.GetListIdForCurrentUserAsync(options);

            options = (options != null) ?
                new GetOptions(options) :
                new();
            options.Filters["StoreId"] = storesId;

            return options;
        }

        public async Task<Int64> GetCountForCurrentUserAsync(GetOptions? options = null)
            => await GetCountAsync(await GetFilterForCurrentUserAsync(options));

        public async Task<IEnumerable<Int64>> GetListIdForCurrentUserAsync(GetOptions? options = null)
            => await GetListIdAsync(await GetFilterForCurrentUserAsync(options));
    }
}


using backend_shop.Entities;
using backend_shop.Exceptions;
using backend_shop.IServices;
using RFOperators;
using RFService.ILibs;
using RFService.IRepo;
using RFService.Libs;
using RFService.Repo;
using RFService.Services;

namespace backend_shop.Service
{
    public class ItemService(
        IRepo<Item> repo,
        IStoreService storeService,
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

            var store = await storeService.GetSingleOrDefaultForIdAsync(
                    data.StoreId,
                    new QueryOptions
                    {
                        Join = { { "Commerce" } },
                        Switches = { { "IncludeDisabled", true } },
                    }
                )
                ?? throw new StoreDoesNotExistException();

            if (store.Commerce == null)
                throw new CommerceDoesNotExistException();

            var totalStoresCount = await GetCountForCurrentUserAsync(new QueryOptions { Filters = { { "IsEnabled", null } } });
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

            data.InheritedIsEnabled = store.IsEnabled && store.Commerce.IsEnabled;
            data.Location = store.Location;

            return data;
        }

        public override async Task<IDataDictionary> ValidateForUpdateAsync(IDataDictionary data, QueryOptions options)
        {
            data = await base.ValidateForUpdateAsync(data, options);

            if (data.TryGetValue("IsEnabled", out var isEnabledValue)
                && isEnabledValue is bool isEnabled && isEnabled)
            {
                var getOptions = new QueryOptions(options)
                {
                    Switches = { { "IncludeDisabled", true } }
                };
                _ = await GetSingleOrDefaultAsync(getOptions)
                    ?? throw new ItemDoesNotExistException();

                var enabledItemsCount = await GetCountForCurrentUserAsync();
                var enabledItemsMax = await userPlanService.GetMaxEnabledItemsForCurrentUser();
                if (enabledItemsCount >= enabledItemsMax)
                    throw new MaxEnabledItemsLimitReachedException();
            }

            return data;
        }

        public async Task<bool> CheckForUuidAndCurrentUserAsync(Guid uuid, QueryOptions? options = null)
        {
            var storesId = await storeService.GetListIdForCurrentUserAsync(options);

            options ??= new ();
            options.Switches["IncludeDisabled"] = true;
            options.AddFilter("Uuid", uuid);
            options.AddFilter("StoreId", storesId);
            _ = await GetSingleOrDefaultAsync(options)
                ?? throw new ItemDoesNotExistException();

            return true;
        }

        public async Task<QueryOptions> GetFilterForCurrentUserAsync(QueryOptions? options = null)
        {
            var storesId = await storeService.GetListIdForCurrentUserAsync(options);

            options = (options != null) ?
                new QueryOptions(options) :
                new();
            options.AddFilter("StoreId", storesId);

            return options;
        }

        public async Task<Int64> GetCountForCurrentUserAsync(QueryOptions? options = null)
            => await GetCountAsync(await GetFilterForCurrentUserAsync(options));

        public async Task<IEnumerable<Int64>> GetListIdForCurrentUserAsync(QueryOptions? options = null)
            => await GetListIdAsync(await GetFilterForCurrentUserAsync(options));

        public async Task<IEnumerable<Guid>> GetListUuidForCurrentUserAsync(QueryOptions? options = null)
            => await GetListUuidAsync(await GetFilterForCurrentUserAsync(options));

        public (QueryOptions, DataDictionary) GetOptionsForUpdateInherited(QueryOptions? options = null)
        {
            options ??= new();
            options.Include("Store", "store");
            options.Include(
                "Commerce",
                "commerce",
                entity: typeof(Commerce),
                on: Op.Eq("commerce.Id", Op.Column("store.CommerceId"))
            );

            var data = new DataDictionary
            {
                { "InheritedIsEnabled",
                    Op.And(
                        Op.Eq("store.IsEnabled", true),
                        Op.IsNull("store.DeletedAt"),
                        Op.Eq("commerce.IsEnabled", true),
                        Op.IsNull("commerce.DeletedAt")
                    )
                },
            };

            return (options, data);
        }

        public async Task<int> UpdateInheritedForUuid(Guid uuid, QueryOptions? options = null)
        {
            (options, DataDictionary data) = GetOptionsForUpdateInherited(options);
            options.AddFilter("uuid", uuid);

            return await UpdateAsync(data, options);
        }

        public async Task<int> UpdateInheritedForStoreUuid(Guid storeUuid, QueryOptions? options = null)
        {
            (options, DataDictionary data) = GetOptionsForUpdateInherited(options);
            options.AddFilter("store.Uuid", storeUuid);
            data["Location"] = Op.Column("store.Location");

            return await UpdateAsync(data, options);
        }

        public async Task<int> UpdateInheritedForCommerceUuid(Guid commerceUuid, QueryOptions? options = null)
        {
            (options, DataDictionary data) = GetOptionsForUpdateInherited(options);
            options.AddFilter("commerce.Uuid", commerceUuid);

            return await UpdateAsync(data, options);
        }
    }
}
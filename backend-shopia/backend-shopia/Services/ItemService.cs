using backend_shopia.DTO;
using backend_shopia.Entities;
using backend_shopia.Exceptions;
using backend_shopia.IServices;
using RFAuth.Exceptions;
using RFOperators;
using RFService.ILibs;
using RFService.IRepo;
using RFService.Libs;
using RFService.Repo;
using RFService.Services;
using System.Text.Json;

namespace backend_shopia.Services
{
    public class ItemService(
        IRepo<Item> repo,
        IServiceProvider serviceProvider
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

            if (data.Store == null)
            {
                var storeService = serviceProvider.GetRequiredService<IStoreService>();
                data.Store = await storeService.GetSingleOrDefaultForIdAsync(
                        data.StoreId,
                        new QueryOptions
                        {
                            Join = { { "Commerce" } },
                            Switches = { { "IncludeDisabled", true } },
                        }
                    )
                    ?? throw new StoreDoesNotExistException();
            }

            if (data.Store.Commerce == null)
            {
                var commerceService = serviceProvider.GetRequiredService<ICommerceService>();
                data.Store.Commerce = await commerceService.GetSingleOrDefaultForIdAsync(
                        data.Store.CommerceId,
                        new QueryOptions
                        {
                            Switches = { { "IncludeDisabled", true } },
                        }
                    )
                    ?? throw new CommerceDoesNotExistException();
            }

            var userPlanService = serviceProvider.GetRequiredService<IUserPlanService>();
            var limits = await userPlanService.GetLimitsForCurrentUserAsync();

            var totalItemsCount = await GetCountForCurrentUserAsync(new QueryOptions { Filters = { { "IsEnabled", null } } });
            if (totalItemsCount >= limits[PlanLimitName.MaxTotalItems])
                throw new TotalItemsLimitReachedException();

            var enabledItemsCount = await GetCountForCurrentUserAsync();
            var enabledItemsMax = limits[PlanLimitName.MaxEnabledItems];
            if (data.IsEnabled && enabledItemsCount >= enabledItemsMax
                || enabledItemsCount > enabledItemsMax
            )
            {
                throw new MaxEnabledItemsLimitReachedException();
            }

            data.InheritedIsEnabled = data.Store.IsEnabled && data.Store.Commerce.IsEnabled;

            data.Embedding = await GetEmbedding(data);

            return data;
        }

        public async Task<float[]> GetEmbedding(Item data)
        {
            if (data.Store == null)
            {
                var storeService = serviceProvider.GetRequiredService<IStoreService>();
                data.Store = await storeService.GetSingleOrDefaultForIdAsync(
                        data.StoreId,
                        new QueryOptions
                        {
                            Join = { { "Commerce" } },
                            Switches = { { "IncludeDisabled", true } },
                        }
                    )
                    ?? throw new StoreDoesNotExistException();
            }

            if (data.Store.Commerce == null)
            {
                var commerceService = serviceProvider.GetRequiredService<ICommerceService>();
                data.Store.Commerce = await commerceService.GetSingleOrDefaultForIdAsync(
                        data.Store.CommerceId,
                        new QueryOptions
                        {
                            Switches = { { "IncludeDisabled", true } },
                        }
                    )
                    ?? throw new CommerceDoesNotExistException();
            }

            var embeddingData = new
            {
                data.Name,
                data.Description,
                data.Category,
                Store = data.Store?.Name,
                Commerce = data.Store?.Commerce?.Name,
                data.Price,
                data.IsPresent,
                data.MinAge,
                data.MaxAge,
            };
            var embeddingService = serviceProvider.GetRequiredService<IEmbeddingService>();
            var embedding = await embeddingService.GetAsync(JsonSerializer.Serialize(embeddingData));

            return embedding;
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

                var userPlanService = serviceProvider.GetRequiredService<IUserPlanService>();
                var limits = await userPlanService.GetLimitsForCurrentUserAsync();

                var enabledItemsCount = await GetCountForCurrentUserAsync();
                var enabledItemsMax = limits[PlanLimitName.MaxEnabledItems];
                if (enabledItemsCount >= enabledItemsMax)
                    throw new MaxEnabledItemsLimitReachedException();
            }

            return data;
        }

        public override async Task<int> UpdateAsync(IDataDictionary data, QueryOptions options)
        {
            var updatedRows = await base.UpdateAsync(data, options);
            if (updatedRows > 0)
            {
                options ??= new QueryOptions();
                options.Switches["IncludeDisabled"] = true;
                options.Include("Store", "store");
                options.Include(
                    "Commerce",
                    "commerce",
                    entity: typeof(Commerce),
                    on: Op.Eq("commerce.Id", Op.Column("store.CommerceId"))
                );
                var items = await GetListAsync(options);
                foreach (var item in items)
                {
                    var embedding = await GetEmbedding(item);
                    await base.UpdateAsync(
                        new DataDictionary { { "Embedding", embedding } },
                        new QueryOptions { Filters = { { "Id", item.Id } } }
                    );
                }
            }

            return updatedRows;
        }

        public async Task<bool> CheckForUuidAndCurrentUserAsync(Guid uuid, QueryOptions? options = null)
        {
            var storeService = serviceProvider.GetRequiredService<IStoreService>();
            var storesId = await storeService.GetListIdForCurrentUserAsync(options);

            options ??= new ();
            options.Switches["IncludeDisabled"] = true;
            options.AddFilter("Uuid", uuid);
            options.AddFilter("StoreId", storesId);
            _ = await GetSingleOrDefaultAsync(options)
                ?? throw new ItemDoesNotExistException();

            return true;
        }

        public async Task<QueryOptions> GetFilterForOwnerIdAsync(Int64 ownerId, QueryOptions? options = null)
        {
            var storeService = serviceProvider.GetRequiredService<IStoreService>();
            var storesId = await storeService.GetListIdForOwnerIdAsync(ownerId, options);

            options = (options != null) ?
                new QueryOptions(options) :
                new();
            options.AddFilter("StoreId", storesId);

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

        public async Task<IEnumerable<Int64>> GetListIdForOwnerIdAsync(Int64 ownerId, QueryOptions? options = null)
            => await GetListIdAsync(await GetFilterForOwnerIdAsync(ownerId, options));

        public async Task<IEnumerable<Int64>> GetListIdForCurrentUserAsync(QueryOptions? options = null)
            => await GetListIdForOwnerIdAsync(GetCurrentUserId(), options);

        public async Task<IEnumerable<Guid>> GetListUuidForCurrentUserAsync(QueryOptions? options = null)
            => await GetListUuidAsync(await GetFilterForOwnerIdAsync(GetCurrentUserId(), options));

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
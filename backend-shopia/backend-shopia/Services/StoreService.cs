using RFService.Services;
using RFService.IRepo;
using backend_shopia.Entities;
using backend_shopia.IServices;
using backend_shopia.Exceptions;
using RFService.Repo;
using RFAuth.Exceptions;
using RFService.ILibs;
using backend_shopia.DTO;

namespace backend_shopia.Services
{
    public class StoreService(
        IRepo<Store> repo,
        IServiceProvider serviceProvider
    )
        : ServiceSoftDeleteTimestampsIdUuidEnabledName<IRepo<Store>, Store>(repo),
            IStoreService
    {
        public override async Task<Store> ValidateForCreationAsync(Store data)
        {
            data = await base.ValidateForCreationAsync(data);

            if (string.IsNullOrWhiteSpace(data.Name))
                throw new NoNameException();

            if (data.CommerceId <= 0)
            {
                data.CommerceId = data.Commerce?.Id ?? 0;
                if (data.CommerceId <= 0)
                    throw new NoCommerceException();
            }

            var commerceService = serviceProvider.GetRequiredService<ICommerceService>();
            _ = await commerceService.GetSingleOrDefaultForIdAsync(data.CommerceId)
                ?? throw new CommerceDoesNotExistException();

            var existent = await GetSingleOrDefaultAsync(new QueryOptions
            {
                Filters = {
                    { "CommerceId", data.CommerceId},
                    { "Name", data.Name }
                }
            });

            if (existent != null)
                throw new AStoreForThatNameAlreadyExistException();

            var userPlanService = serviceProvider.GetRequiredService<IUserPlanService>();
            var limits = await userPlanService.GetLimitsForCurrentUserAsync();

            var totalStoresCount = await GetCountForCurrentUserAsync(new QueryOptions { Filters = { { "IsEnabled", null } } });
            if (totalStoresCount >= limits[PlanLimitName.MaxTotalStores])
                throw new TotalStoresLimitReachedException();

            var enabledStoresCount = await GetCountForCurrentUserAsync();
            var enabledStoresMax = limits[PlanLimitName.MaxEnabledStores];
            if (data.IsEnabled && enabledStoresCount >= enabledStoresMax
                || enabledStoresCount > enabledStoresMax
            )
            {
                throw new MaxEnabledStoresLimitReachedException();
            }

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
                    ?? throw new StoreDoesNotExistException();

                var userPlanService = serviceProvider.GetRequiredService<IUserPlanService>();
                var limits = await userPlanService.GetLimitsForCurrentUserAsync();

                var enabledStoresCount = await GetCountForCurrentUserAsync();
                var enabledStoresMax = limits[PlanLimitName.MaxEnabledStores];
                if (enabledStoresCount >= enabledStoresMax)
                    throw new MaxEnabledStoresLimitReachedException();
            }

            return data;
        }

        public async Task<bool> CheckForUuidAndCurrentUserAsync(Guid uuid, QueryOptions? options = null)
        {
            var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
            var httpContext = httpContextAccessor.HttpContext
                ?? throw new NoAuthorizationHeaderException();

            var ownerId = (httpContext.Items["UserId"] as Int64?)
                ?? throw new NoSessionUserDataException();

            if (ownerId <= 0)
                throw new NoSessionUserDataException();

            options ??= QueryOptions.CreateFromQuery(httpContext);
            options.Include("Commerce", "commerce");
            options.Switches["IncludeDisabled"] = true;
            options.AddFilter("Uuid", uuid);
            options.AddFilter("commerce.OwnerId", ownerId);
            _ = await GetSingleOrDefaultAsync(options)
                ?? throw new StoreDoesNotExistException();

            return true;
        }

        public async Task<QueryOptions> GetFilterForOwnerIdAsync(Int64 ownerId, QueryOptions? options = null)
        {
            var commerceService = serviceProvider.GetRequiredService<ICommerceService>();
            var commercesId = await commerceService.GetListIdForOwnerIdAsync(ownerId, options);

            options = (options != null) ?
                new QueryOptions(options) :
                new();
            options.AddFilter("CommerceId", commercesId);

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
    }
}


using RFService.Services;
using RFService.IRepo;
using backend_shop.Entities;
using backend_shop.IServices;
using backend_shop.Exceptions;
using RFService.Repo;
using RFAuth.Exceptions;
using RFService.ILibs;

namespace backend_shop.Service
{
    public class StoreService(
        IRepo<Store> repo,
        IBusinessService businessService,
        IUserPlanService userPlanService,
        IHttpContextAccessor httpContextAccessor
    )
        : ServiceSoftDeleteTimestampsIdUuidEnabledName<IRepo<Store>, Store>(repo),
            IStoreService
    {
        public override async Task<Store> ValidateForCreationAsync(Store data)
        {
            data = await base.ValidateForCreationAsync(data);

            if (string.IsNullOrWhiteSpace(data.Name))
                throw new NoNameException();

            if (data.BusinessId <= 0)
            {
                data.BusinessId = data.Business?.Id ?? 0;
                if (data.BusinessId <= 0)
                    throw new NoBusinessException();
            }

            _ = await businessService.GetSingleOrDefaultForIdAsync(data.BusinessId)
                ?? throw new BusinessDoesNotExistException();

            var existent = await GetSingleOrDefaultAsync(new GetOptions
            {
                Filters = {
                    { "BusinessId", data.BusinessId},
                    { "Name", data.Name }
                }
            });

            if (existent != null)
                throw new AStoreForThatNameAlreadyExistException();

            var totalStoresCount = await GetCountForCurrentUserAsync(new GetOptions { Filters = { { "IsEnabled", null } } });
            if (totalStoresCount >= (await userPlanService.GetMaxTotalStoresForCurrentUser()))
                throw new TotalStoresLimitReachedException();

            var enabledStoresCount = await GetCountForCurrentUserAsync();
            var enabledStoresMax = await userPlanService.GetMaxEnabledStoresForCurrentUser();
            if (data.IsEnabled && enabledStoresCount >= enabledStoresMax
                || enabledStoresCount > enabledStoresMax
            )
            {
                throw new MaxEnabledStoresLimitReachedException();
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
                    ?? throw new StoreDoesNotExistException();

                var enabledStoresCount = await GetCountForCurrentUserAsync();
                var enabledStoresMax = await userPlanService.GetMaxEnabledStoresForCurrentUser();
                if (enabledStoresCount >= enabledStoresMax)
                    throw new MaxEnabledStoresLimitReachedException();
            }

            return data;
        }

        public async Task<bool> CheckForUuidAndCurrentUserAsync(Guid uuid, GetOptions? options = null)
        {
            var httpContext = httpContextAccessor.HttpContext
                ?? throw new NoAuthorizationHeaderException();

            var ownerId = (httpContext.Items["UserId"] as Int64?)
                ?? throw new NoSessionUserDataException();

            if (ownerId <= 0)
                throw new NoSessionUserDataException();

            options ??= GetOptions.CreateFromQuery(httpContext);
            options.Include("Business", "business");
            options.Filters["IsEnabled"] = null;
            options.Filters["Uuid"] = uuid;
            options.Filters["business.OwnerId"] = ownerId;
            _ = await GetSingleOrDefaultAsync(options)
                ?? throw new StoreDoesNotExistException();

            return true;
        }

        public async Task<GetOptions> GetFilterForCurrentUserAsync(GetOptions? options = null)
        {
            var businessesId = await businessService.GetListIdForCurrentUserAsync(options);

            options = (options != null) ?
                new GetOptions(options) :
                new();
            options.Filters["BusinessId"] = businessesId;

            return options;
        }

        public async Task<Int64> GetCountForCurrentUserAsync(GetOptions? options = null)
            => await GetCountAsync(await GetFilterForCurrentUserAsync(options));

        public async Task<IEnumerable<Int64>> GetListIdForCurrentUserAsync(GetOptions? options = null)
            => await GetListIdAsync(await GetFilterForCurrentUserAsync(options));
    }
}


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

            var existent = await GetSingleOrDefaultAsync(new GetOptions
            {
                Filters = {
                    { "BusinessId", data.BusinessId},
                    { "Name", data.Name }
                }
            });

            if (existent != null)
                throw new AStoreForThatNameAlreadyExistException();

            var business = await businessService.GetSingleForIdAsync(data.BusinessId);

            var totalStoresCount = await GetCountAsync(new GetOptions
            {
                Join = {
                    { "Business", new From("business") },
                },
                Filters = {
                    { "BusinessId", data.BusinessId },
                    { "IsEnabled", null },
                    { "Business.IsEnabled", null },
                    { "Business.OwnerId", business.OwnerId }
                }
            });
            if (totalStoresCount >= (await userPlanService.GetMaxTotalStoresForCurrentUser()))
                throw new TotalStoresLimitReachedException();

            var enabledStoresCount = await GetCountAsync(new GetOptions
            {
                Join = {
                    { "Business", new From("business") },
                },
                Filters = {
                    { "BusinessId", data.BusinessId },
                    { "Business.OwnerId", business.OwnerId }
                }
            });
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
                var business = await GetSingleOrDefaultAsync(getOptions)
                    ?? throw new BusinessDoesNotExistException();

                var enabledBusinessesCount = await GetCountAsync(new GetOptions { Filters = { { "BusinessId", business.BusinessId } } });
                var enabledBusinessesMax = await userPlanService.GetMaxEnabledBusinessesForCurrentUser();
                if (enabledBusinessesCount >= enabledBusinessesMax)
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
            options.Filters["IsEnabled"] = null;
            options.Filters["Uuid"] = uuid;
            options.Include("Business");
            var store = await GetSingleOrDefaultAsync(options)
                ?? throw new StoreDoesNotExistException();

            if (store.Business?.OwnerId != ownerId)
                throw new StoreDoesNotExistException();

            return true;
        }
    }
}


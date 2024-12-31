using RFService.Services;
using RFService.IRepo;
using backend_shop.Entities;
using backend_shop.IServices;
using backend_shop.Exceptions;
using RFService.Repo;
using RFAuth.Exceptions;
using RFAuth.Entities;

namespace backend_shop.Service
{
    public class StoreService(
        IRepo<Store> repo,
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

            var enabledStoresCount = await GetCountAsync(new GetOptions { Filters = { { "BusinessId", data.BusinessId } } });

            if (enabledStoresCount >= (await userPlanService.GetMaxEnabledStoresForCurrentUser()))
                throw new BusinessLimitReachedException();

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
            options.AddFilter("Uuid", uuid);
            options.Include("Business");
            var store = await GetSingleOrDefaultAsync(options)
                ?? throw new StoreDoesNotExistException();

            if (store.Business?.OwnerId != ownerId)
                throw new StoreDoesNotExistException();

            return true;
        }
    }
}


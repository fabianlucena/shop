using RFService.Services;
using RFService.IRepo;
using backend_shop.Entities;
using backend_shop.IServices;
using backend_shop.Exceptions;
using RFService.Repo;
using RFAuth.Exceptions;

namespace backend_shop.Service
{
    public class BusinessService(
        IRepo<Business> repo,
        IUserPlanService userPlanService,
        IHttpContextAccessor httpContextAccessor
    )
        : ServiceSoftDeleteTimestampsIdUuidEnabledName<IRepo<Business>, Business>(repo),
            IBusinessService
    {
        public override async Task<Business> ValidateForCreationAsync(Business data)
        {
            data = await base.ValidateForCreationAsync(data);

            if (string.IsNullOrWhiteSpace(data.Name))
                throw new NoNameException();

            if (data.OwnerId <= 0)
            {
                data.OwnerId = data.Owner?.Id ?? 0;
                if (data.OwnerId <= 0)
                    throw new NoOwnerException();
            }

            var existent = await GetSingleOrDefaultAsync(new GetOptions
            {
                Filters = {
                    { "OwnerId", data.OwnerId},
                    { "Name", data.Name }
                }
            });

            if (existent != null)
                throw new ABusinessForThatNameAlreadyExistException();

            var enabledBusinessCount = await GetCountAsync(new GetOptions { Filters = { { "OwnerId", data.OwnerId } } });

            if (enabledBusinessCount >= (await userPlanService.GetMaxEnabledBusinessForCurrentUser()))
                throw new BusinessLimitReachedException();

            return data;
        }

        public async Task<bool> CheckForUuidAndCurrentUserAsync(Guid uuid, GetOptions? options = null)
        {
            var httpContext = httpContextAccessor.HttpContext
                ?? throw new NoAuthorizationHeaderException();

            var ownerId = (httpContext.Items["UserId"] as Int64?)
                ?? throw new NoAuthorizationHeaderException();

            options ??= GetOptions.CreateFromQuery(httpContext);
            options.AddFilter("OwnerId", ownerId);
            options.AddFilter("Uuid", uuid);

            if (await GetSingleOrDefaultAsync(options) != null)
                return true;

            throw new BusinessDoesNotExistException();
        }

        public async Task<IEnumerable<Int64>> GetListIdForCurrentUserAsync(GetOptions? options = null)
        {
            var httpContext = httpContextAccessor.HttpContext
                ?? throw new NoAuthorizationHeaderException();

            var ownerId = (httpContext.Items["UserId"] as Int64?)
                ?? throw new NoSessionUserDataException();

            if (ownerId <= 0)
                throw new NoSessionUserDataException();

            options ??= GetOptions.CreateFromQuery(httpContext);
            options.AddFilter("OwnerId", ownerId);

            return await GetListIdAsync(options);
        }
    }
}


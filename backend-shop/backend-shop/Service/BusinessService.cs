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

            var totalBusinessesCount = await GetCountForCurrentUserAsync(new GetOptions { Filters = { { "IsEnabled", null } } });
            if (totalBusinessesCount >= (await userPlanService.GetMaxTotalBusinessesForCurrentUser()))
                throw new TotalBusinessesLimitReachedException();

            var enabledBusinessesCount = await GetCountForCurrentUserAsync();
            var enabledBusinessesMax = await userPlanService.GetMaxEnabledBusinessesForCurrentUser();
            if (data.IsEnabled && enabledBusinessesCount >= enabledBusinessesMax
                || enabledBusinessesCount > enabledBusinessesMax
            )
            {
                throw new MaxEnabledBusinessesLimitReachedException();
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
                    ?? throw new BusinessDoesNotExistException();

                var enabledBusinessesCount = await GetCountForCurrentUserAsync();
                var enabledBusinessesMax = await userPlanService.GetMaxEnabledBusinessesForCurrentUser();
                if (enabledBusinessesCount >= enabledBusinessesMax)
                    throw new MaxEnabledBusinessesLimitReachedException();
            }

            return data;
        }

        public async Task<bool> CheckForUuidAndCurrentUserAsync(Guid uuid, GetOptions? options = null)
        {
            var httpContext = httpContextAccessor.HttpContext
                ?? throw new NoAuthorizationHeaderException();

            var ownerId = (httpContext.Items["UserId"] as Int64?)
                ?? throw new NoAuthorizationHeaderException();

            options ??= GetOptions.CreateFromQuery(httpContext);
            options.Filters["IsEnabled"] = null;
            options.Filters["OwnerId"] = ownerId;
            options.Filters["Uuid"] = uuid;

            if (await GetSingleOrDefaultAsync(options) != null)
                return true;

            throw new BusinessDoesNotExistException();
        }

        public Task<GetOptions> GetFilterForCurrentUserAsync(GetOptions? options = null)
        {
            var httpContext = httpContextAccessor.HttpContext
                ?? throw new NoAuthorizationHeaderException();

            var ownerId = (httpContext.Items["UserId"] as Int64?)
                ?? throw new NoSessionUserDataException();

            if (ownerId <= 0)
                throw new NoSessionUserDataException();

            options ??= GetOptions.CreateFromQuery(httpContext);
            options.Filters["OwnerId"] = ownerId;

            return Task.FromResult(options);
        }

        public async Task<Int64> GetCountForCurrentUserAsync(GetOptions? options = null)
            => await GetCountAsync(await GetFilterForCurrentUserAsync(options));

        public async Task<IEnumerable<Int64>> GetListIdForCurrentUserAsync(GetOptions? options = null)
            => await GetListIdAsync(await GetFilterForCurrentUserAsync(options));
    }
}


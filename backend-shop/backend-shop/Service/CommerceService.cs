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
    public class CommerceService(
        IRepo<Commerce> repo,
        IUserPlanService userPlanService,
        IHttpContextAccessor httpContextAccessor
    )
        : ServiceSoftDeleteTimestampsIdUuidEnabledName<IRepo<Commerce>, Commerce>(repo),
            ICommerceService
    {
        public override async Task<Commerce> ValidateForCreationAsync(Commerce data)
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
                throw new ACommerceForThatNameAlreadyExistException();

            var totalCommercesCount = await GetCountForCurrentUserAsync(new GetOptions { Filters = { { "IsEnabled", null } } });
            if (totalCommercesCount >= (await userPlanService.GetMaxTotalCommercesForCurrentUser()))
                throw new TotalCommercesLimitReachedException();

            var enabledCommercesCount = await GetCountForCurrentUserAsync();
            var enabledCommercesMax = await userPlanService.GetMaxEnabledCommercesForCurrentUser();
            if (data.IsEnabled && enabledCommercesCount >= enabledCommercesMax
                || enabledCommercesCount > enabledCommercesMax
            )
            {
                throw new MaxEnabledCommercesLimitReachedException();
            }

            return data;
        }

        public override async Task<IDataDictionary> ValidateForUpdateAsync(IDataDictionary data, GetOptions options)
        {
            data = await base.ValidateForUpdateAsync(data, options);

            if (data.TryGetValue("IsEnabled", out var isEnabledValue)
                && isEnabledValue is bool isEnabled && isEnabled)
            {
                var getOptions = new GetOptions(options)
                {
                    IncludeDisabled = true
                };
                _ = await GetSingleOrDefaultAsync(getOptions)
                    ?? throw new CommerceDoesNotExistException();

                var enabledCommercesCount = await GetCountForCurrentUserAsync();
                var enabledCommercesMax = await userPlanService.GetMaxEnabledCommercesForCurrentUser();
                if (enabledCommercesCount >= enabledCommercesMax)
                    throw new MaxEnabledCommercesLimitReachedException();
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
            options.IncludeDisabled = true;
            options.AddFilter("OwnerId", ownerId);
            options.AddFilter("Uuid", uuid);

            if (await GetSingleOrDefaultAsync(options) != null)
                return true;

            throw new CommerceDoesNotExistException();
        }

        public Task<GetOptions> GetFilterForCurrentUserAsync(GetOptions? options = null)
        {
            var httpContext = httpContextAccessor.HttpContext
                ?? throw new NoAuthorizationHeaderException();

            var ownerId = (httpContext.Items["UserId"] as Int64?)
                ?? throw new NoSessionUserDataException();

            if (ownerId <= 0)
                throw new NoSessionUserDataException();

            options = (options != null) ?
                new GetOptions(options) :
                new();
            options.AddFilter("OwnerId", ownerId);

            return Task.FromResult(options);
        }

        public async Task<Int64> GetCountForCurrentUserAsync(GetOptions? options = null)
            => await GetCountAsync(await GetFilterForCurrentUserAsync(options));

        public async Task<IEnumerable<Int64>> GetListIdForCurrentUserAsync(GetOptions? options = null)
            => await GetListIdAsync(await GetFilterForCurrentUserAsync(options));
    }
}


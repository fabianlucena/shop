using backend_shopia.DTO;
using backend_shopia.Entities;
using backend_shopia.Exceptions;
using backend_shopia.IServices;
using RFAuth.Exceptions;
using RFService.ILibs;
using RFService.IRepo;
using RFService.Repo;
using RFService.Services;

namespace backend_shopia.Services
{
    public class CommerceService(
        IRepo<Commerce> repo,
        IServiceProvider serviceProvider
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

            var existent = await GetSingleOrDefaultAsync(new QueryOptions
            {
                Filters = {
                    { "OwnerId", data.OwnerId},
                    { "Name", data.Name }
                }
            });

            if (existent != null)
                throw new ACommerceForThatNameAlreadyExistException();

            var userPlanService = serviceProvider.GetRequiredService<IUserPlanService>();
            var limits = await userPlanService.GetLimitsForCurrentUserAsync();

            var totalCommercesCount = await GetCountForCurrentUserAsync(new QueryOptions { Filters = { { "IsEnabled", null } } });
            if (totalCommercesCount >= limits[PlanLimitName.MaxTotalCommerces])
                throw new TotalCommercesLimitReachedException();

            var enabledCommercesCount = await GetCountForCurrentUserAsync();
            var enabledCommercesMax = limits[PlanLimitName.MaxEnabledCommerces];
            if (data.IsEnabled && enabledCommercesCount >= enabledCommercesMax
                || enabledCommercesCount > enabledCommercesMax
            )
            {
                throw new MaxEnabledCommercesLimitReachedException();
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
                    ?? throw new CommerceDoesNotExistException();

                var userPlanService = serviceProvider.GetRequiredService<IUserPlanService>();
                var limits = await userPlanService.GetLimitsForCurrentUserAsync();

                var enabledCommercesCount = await GetCountForCurrentUserAsync();
                var enabledCommercesMax = limits[PlanLimitName.MaxEnabledCommerces];
                if (enabledCommercesCount >= enabledCommercesMax)
                    throw new MaxEnabledCommercesLimitReachedException();
            }

            return data;
        }

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

        public async Task<bool> CheckForUuidAndCurrentUserAsync(Guid uuid, QueryOptions? options = null)
        {
            var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
            var httpContext = httpContextAccessor.HttpContext
                ?? throw new NoAuthorizationHeaderException();

            var ownerId = GetCurrentUserId();

            options ??= QueryOptions.CreateFromQuery(httpContext);
            options.Switches["IncludeDisabled"] = true;
            options.AddFilter("OwnerId", ownerId);
            options.AddFilter("Uuid", uuid);

            if (await GetSingleOrDefaultAsync(options) != null)
                return true;

            throw new CommerceDoesNotExistException();
        }

        public QueryOptions GetFilterForOwnerIdAsync(Int64 ownerId, QueryOptions? options = null)
        {
            options = (options != null) ?
                new QueryOptions(options) :
                new();
            options.AddFilter("OwnerId", ownerId);

            return options;
        }

        public async Task<int> GetCountForOwnerIdAsync(Int64 ownerId, QueryOptions? options = null)
            => await GetCountAsync(GetFilterForOwnerIdAsync(ownerId, options));

        public async Task<int> GetCountForCurrentUserAsync(QueryOptions? options = null)
            => await GetCountForOwnerIdAsync(GetCurrentUserId(), options);

        public async Task<IEnumerable<Int64>> GetListIdForOwnerIdAsync(Int64 ownerId, QueryOptions? options = null)
            => await GetListIdAsync(GetFilterForOwnerIdAsync(ownerId, options));

        public async Task<IEnumerable<Int64>> GetListIdForCurrentUserAsync(QueryOptions? options = null)
            => await GetListIdForOwnerIdAsync(GetCurrentUserId(), options);
    }
}


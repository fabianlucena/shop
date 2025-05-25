using RFService.Services;
using RFService.IRepo;
using backend_shop.Entities;
using backend_shop.IServices;
using RFService.Repo;
using RFAuth.Exceptions;
using RFOperators;

namespace backend_shop.Service
{
    public class UserPlanService(
        IRepo<UserPlan> repo,
        IPlanService planService,
        IHttpContextAccessor httpContextAccessor
    )
        : ServiceTimestampsIdUuidEnabled<IRepo<UserPlan>, UserPlan>(repo),
            IUserPlanService
    {
        public async Task<int> GetMaxTotalCommercesForUserId(Int64 userId)
        {
            var options = new GetOptions
            {
                Join = { { "Plan", "plan" } },
                Filters = {
                    { "UserId", userId },
                    { Op.IsNotNull("plan.MaxTotalCommerces") },
                    { Op.GE("ExpirationDate", DateTime.UtcNow) },
                },
                OrderBy = { "plan.MaxTotalCommerces DESC" },
                Top = 1,
            };

            return (await GetFirstOrDefaultAsync(options))?.Plan?.MaxTotalCommerces
                ?? (await planService.GetBaseAsync()).MaxTotalCommerces
                ?? default;
        }

        public async Task<int> GetMaxEnabledCommercesForUserId(Int64 userId)
        {
            var options = new GetOptions
            {
                Join = { { "Plan", "plan" } },
                Filters = {
                    { "UserId", userId },
                    { Op.IsNotNull("plan.MaxEnabledCommerces") },
                    { Op.GE("ExpirationDate", DateTime.UtcNow) },
                },
                OrderBy = { "plan.MaxEnabledCommerces DESC" },
                Top = 1,
            };

            return (await GetFirstOrDefaultAsync(options))?.Plan?.MaxEnabledCommerces
                ?? (await planService.GetBaseAsync()).MaxEnabledCommerces
                ?? default;
        }

        public async Task<int> GetMaxTotalStoresForUserId(Int64 userId)
        {
            var options = new GetOptions
            {
                Join = { { "Plan", "plan" } },
                Filters = {
                    { "UserId", userId },
                    { Op.IsNotNull("plan.MaxEnabledCommerces") },
                    { Op.GE("ExpirationDate", DateTime.UtcNow) },
                },
                OrderBy = { "plan.MaxTotalStores DESC" },
                Top = 1,
            };

            return (await GetFirstOrDefaultAsync(options))?.Plan?.MaxTotalStores
                ?? (await planService.GetBaseAsync()).MaxTotalStores
                ?? default;
        }

        public async Task<int> GetMaxEnabledStoresForUserId(Int64 userId)
        {
            var options = new GetOptions
            {
                Join = { { "Plan", "plan" } },
                Filters = {
                    { "UserId", userId },
                    { Op.IsNotNull("plan.MaxEnabledStores") },
                    { Op.GE("ExpirationDate", DateTime.UtcNow) },
                },
                OrderBy = { "plan.MaxEnabledStores DESC" },
                Top = 1,
            };

            return (await GetFirstOrDefaultAsync(options))?.Plan?.MaxEnabledStores
                ?? (await planService.GetBaseAsync()).MaxEnabledStores
                ?? default;
        }

        public async Task<int> GetMaxTotalItemsForUserId(Int64 userId)
        {
            var options = new GetOptions
            {
                Join = { { "Plan", "plan" } },
                Filters = {
                    { "UserId", userId },
                    { Op.IsNotNull("plan.MaxTotalItems") },
                    { Op.GE("ExpirationDate", DateTime.UtcNow) },
                },
                OrderBy = { "plan.MaxTotalItems DESC" },
                Top = 1,
            };

            return (await GetFirstOrDefaultAsync(options))?.Plan?.MaxTotalItems
                ?? (await planService.GetBaseAsync()).MaxTotalItems
                ?? default;
        }

        public async Task<int> GetMaxEnabledItemsForUserId(Int64 userId)
        {
            var options = new GetOptions
            {
                Join = { { "Plan", "plan" } },
                Filters = {
                    { "UserId", userId },
                    { Op.IsNotNull("plan.MaxEnabledItems") },
                    { Op.GE("ExpirationDate", DateTime.UtcNow) },
                },
                OrderBy = { "plan.MaxEnabledItems DESC" },
                Top = 1,
            };

            return (await GetFirstOrDefaultAsync(options))?.Plan?.MaxEnabledItems
                ?? (await planService.GetBaseAsync()).MaxEnabledItems
                ?? default;
        }

        public async Task<int> GetMaxTotalCommercesForCurrentUser()
        {
            var httpContext = httpContextAccessor.HttpContext
                ?? throw new NoAuthorizationHeaderException();

            var userId = (httpContext.Items["UserId"] as Int64?)
                ?? throw new NoSessionUserDataException();

            if (userId <= 0)
                throw new NoSessionUserDataException();

            return await GetMaxTotalCommercesForUserId(userId);
        }

        public async Task<int> GetMaxEnabledCommercesForCurrentUser()
        {
            var httpContext = httpContextAccessor.HttpContext
                ?? throw new NoAuthorizationHeaderException();

            var userId = (httpContext.Items["UserId"] as Int64?)
                ?? throw new NoSessionUserDataException();

            if (userId <= 0)
                throw new NoSessionUserDataException();

            return await GetMaxEnabledCommercesForUserId(userId);
        }

        public async Task<int> GetMaxTotalStoresForCurrentUser()
        {
            var httpContext = httpContextAccessor.HttpContext
                ?? throw new NoAuthorizationHeaderException();

            var userId = (httpContext.Items["UserId"] as Int64?)
                ?? throw new NoSessionUserDataException();

            if (userId <= 0)
                throw new NoSessionUserDataException();

            return await GetMaxTotalStoresForUserId(userId);
        }

        public async Task<int> GetMaxEnabledStoresForCurrentUser()
        {
            var httpContext = httpContextAccessor.HttpContext
                ?? throw new NoAuthorizationHeaderException();

            var userId = (httpContext.Items["UserId"] as Int64?)
                ?? throw new NoSessionUserDataException();

            if (userId <= 0)
                throw new NoSessionUserDataException();

            return await GetMaxEnabledStoresForUserId(userId);
        }

        public async Task<int> GetMaxTotalItemsForCurrentUser()
        {
            var httpContext = httpContextAccessor.HttpContext
                ?? throw new NoAuthorizationHeaderException();

            var userId = (httpContext.Items["UserId"] as Int64?)
                ?? throw new NoSessionUserDataException();

            if (userId <= 0)
                throw new NoSessionUserDataException();

            return await GetMaxTotalItemsForUserId(userId);
        }

        public async Task<int> GetMaxEnabledItemsForCurrentUser()
        {
            var httpContext = httpContextAccessor.HttpContext
                ?? throw new NoAuthorizationHeaderException();

            var userId = (httpContext.Items["UserId"] as Int64?)
                ?? throw new NoSessionUserDataException();

            if (userId <= 0)
                throw new NoSessionUserDataException();

            return await GetMaxEnabledItemsForUserId(userId);
        }
    }
}

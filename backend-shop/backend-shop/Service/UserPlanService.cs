using RFService.Services;
using RFService.IRepo;
using backend_shop.Entities;
using backend_shop.IServices;
using RFService.Repo;
using RFAuth.Exceptions;
using RFService.Operator;

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
        public async Task<int> GetMaxTotalBusinessesForUserId(Int64 userId)
        {
            var options = new GetOptions
            {
                Join = {
                    { "Plan", new From("plan") },
                },
                Filters = {
                    { "UserId", userId },
                    { "[plan].MaxTotalBusinesses", Op.NotNull() },
                    { "ExpirationDate", Op.GE(DateTime.UtcNow) },
                },
                OrderBy = { "[plan].MaxTotalBusinesses DESC" },
                Top = 1,
            };

            return (await GetFirstOrDefaultAsync(options))?.Plan?.MaxTotalBusinesses
                ?? (await planService.GetBaseAsync()).MaxTotalBusinesses
                ?? default;
        }

        public async Task<int> GetMaxEnabledBusinessesForUserId(Int64 userId)
        {
            var options = new GetOptions
            {
                Join = {
                    { "Plan", new From("plan") },
                },
                Filters = {
                    { "UserId", userId },
                    { "[plan].MaxEnabledBusinesses", Op.NotNull() },
                    { "ExpirationDate", Op.GE(DateTime.UtcNow) },
                },
                OrderBy = { "[plan].MaxEnabledBusinesses DESC" },
                Top = 1,
            };

            return (await GetFirstOrDefaultAsync(options))?.Plan?.MaxEnabledBusinesses
                ?? (await planService.GetBaseAsync()).MaxEnabledBusinesses
                ?? default;
        }

        public async Task<int> GetMaxTotalStoresForUserId(Int64 userId)
        {
            var options = new GetOptions
            {
                Join = {
                    { "Plan", new From("plan") },
                },
                Filters = {
                    { "UserId", userId },
                    { "[plan].MaxTotalStores", Op.NotNull() },
                    { "ExpirationDate", Op.GE(DateTime.UtcNow) },
                },
                OrderBy = { "[plan].MaxTotalStores DESC" },
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
                Join = {
                    { "Plan", new From("plan") },
                },
                Filters = {
                    { "UserId", userId },
                    { "[plan].MaxEnabledStores", Op.NotNull() },
                    { "ExpirationDate", Op.GE(DateTime.UtcNow) },
                },
                OrderBy = { "[plan].MaxEnabledStores DESC" },
                Top = 1,
            };

            return (await GetFirstOrDefaultAsync(options))?.Plan?.MaxEnabledStores
                ?? (await planService.GetBaseAsync()).MaxEnabledStores
                ?? default;
        }

        public async Task<int> GetMaxTotalBusinessesForCurrentUser()
        {
            var httpContext = httpContextAccessor.HttpContext
                ?? throw new NoAuthorizationHeaderException();

            var userId = (httpContext.Items["UserId"] as Int64?)
                ?? throw new NoSessionUserDataException();

            if (userId <= 0)
                throw new NoSessionUserDataException();

            return await GetMaxTotalBusinessesForUserId(userId);
        }

        public async Task<int> GetMaxEnabledBusinessesForCurrentUser()
        {
            var httpContext = httpContextAccessor.HttpContext
                ?? throw new NoAuthorizationHeaderException();

            var userId = (httpContext.Items["UserId"] as Int64?)
                ?? throw new NoSessionUserDataException();

            if (userId <= 0)
                throw new NoSessionUserDataException();

            return await GetMaxEnabledBusinessesForUserId(userId);
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
    }
}

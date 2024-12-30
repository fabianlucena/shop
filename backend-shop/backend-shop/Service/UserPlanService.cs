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
        private HttpContext? HttpContext => httpContextAccessor?.HttpContext;

        public async Task<int> GetMaxEnabledBusinessForUserId(Int64 userId)
        {
            var options = new GetOptions
            {
                Join = {
                    { "Plan", new From("plan") },
                },
                Filters = {
                    { "UserId", userId },
                    { "ExpirationDate", Op.GE(DateTime.UtcNow) },
                },
                OrderBy = { "[plan].MaxEnabledBusinesses DESC" },
                Top = 1,
            };

            var userPlan = await GetFirstOrDefaultAsync(options);

            var plan = userPlan?.Plan
                ?? await planService.GetBaseAsync();

            return plan.MaxEnabledBusinesses;
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
                    { "ExpirationDate", Op.GE(DateTime.UtcNow) },
                },
                OrderBy = { "[plan].MaxEnabledStores DESC" },
                Top = 1,
            };

            var userPlan = await GetFirstOrDefaultAsync(options);

            var plan = userPlan?.Plan
                ?? await planService.GetBaseAsync();

            return plan.MaxEnabledStores;
        }

        public async Task<int> GetMaxEnabledBusinessForCurrentUser()
        {
            var userId = HttpContext?.Items["UserId"] as Int64?;
            if (userId == null || userId == 0)
                throw new NoAuthorizationHeaderException();

            return await GetMaxEnabledBusinessForUserId(userId.Value);
        }

        public async Task<int> GetMaxEnabledStoresForCurrentUser()
        {
            var userId = HttpContext?.Items["UserId"] as Int64?;
            if (userId == null || userId == 0)
                throw new NoAuthorizationHeaderException();

            return await GetMaxEnabledStoresForUserId(userId.Value);
        }
    }
}

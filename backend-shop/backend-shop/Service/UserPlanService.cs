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
            var httpContext = httpContextAccessor.HttpContext
                ?? throw new NoAuthorizationHeaderException();

            var userId = (httpContext.Items["UserId"] as Int64?)
                ?? throw new NoSessionUserDataException();

            if (userId <= 0)
                throw new NoSessionUserDataException();

            return await GetMaxEnabledBusinessForUserId(userId);
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

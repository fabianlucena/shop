using backend_shop.DTO;
using backend_shop.Entities;
using backend_shop.IServices;
using RFAuth.Exceptions;
using RFOperators;
using RFService.IRepo;
using RFService.Repo;
using RFService.Services;

namespace backend_shop.Service
{
    public class UserPlanService(
        IRepo<UserPlan> repo,
        IPlanService planService,
        IHttpContextAccessor httpContextAccessor,
        IServiceProvider serviceProvider
    )
        : ServiceTimestampsIdUuidEnabled<IRepo<UserPlan>, UserPlan>(repo),
            IUserPlanService
    {
        public async Task<Plan> GetSinglePlanForCurrentUserAsync()
        {
            var httpContext = httpContextAccessor.HttpContext
                ?? throw new NoAuthorizationHeaderException();

            var ownerId = (httpContext.Items["UserId"] as Int64?)
                ?? throw new NoSessionUserDataException();

            if (ownerId <= 0)
                throw new NoSessionUserDataException();

            var userPlan = await GetFirstOrDefaultAsync(
                new QueryOptions
                {
                    Join = { { "Plan", "plan" } },
                    Filters = {
                        { "UserId", ownerId },
                        { Op.GE("ExpirationDate", DateTime.UtcNow) },
                    },
                    OrderBy = { "ExpirationDate DESC" },
                    Top = 1,
                }
            );

            var plan = userPlan?.Plan ?? new Plan { Name = "" };
            var basePlan = await planService.GetBaseAsync();
            plan.Uuid = plan.Uuid == Guid.Empty ? basePlan.Uuid : plan.Uuid;
            plan.Name = string.IsNullOrWhiteSpace(plan.Name) ? basePlan.Name : plan.Name;
            plan.MaxTotalCommerces ??= basePlan.MaxTotalCommerces;
            plan.MaxEnabledCommerces ??= basePlan.MaxEnabledCommerces;
            plan.MaxTotalStores ??= basePlan.MaxTotalStores;
            plan.MaxEnabledStores ??= basePlan.MaxEnabledStores;
            plan.MaxTotalItems ??= basePlan.MaxTotalItems;
            plan.MaxEnabledItems ??= basePlan.MaxEnabledItems;
            plan.MaxTotalItemsImages ??= basePlan.MaxTotalItemsImages;
            plan.MaxEnabledItemsImages ??= basePlan.MaxEnabledItemsImages;
            plan.MaxAggregattedSizeItemsImages ??= basePlan.MaxAggregattedSizeItemsImages;
            plan.MaxEnabledAggregattedSizeItemsImages ??= basePlan.MaxEnabledAggregattedSizeItemsImages;

            return plan;
        }

        public async Task<UsedPlanDTO> GetUsedPlanForCurrentUserAsync()
        {
            var httpContext = httpContextAccessor.HttpContext
                ?? throw new NoAuthorizationHeaderException();

            var ownerId = (httpContext.Items["UserId"] as Int64?)
                ?? throw new NoSessionUserDataException();

            if (ownerId <= 0)
                throw new NoSessionUserDataException();

            var commerceService = serviceProvider.GetRequiredService<ICommerceService>();
            var storeService = serviceProvider.GetRequiredService<IStoreService>();
            var itemService = serviceProvider.GetRequiredService<IItemService>();
            var itemFileService = serviceProvider.GetRequiredService<IItemFileService>();
            var includeDisabledOptions = new QueryOptions { Switches = { { "IncludeDisabled", true } } };

            var usedPlan = new UsedPlanDTO
            {
                TotalCommercesCount = await commerceService.GetCountForOwnerIdAsync(ownerId, includeDisabledOptions),
                EnabledCommercesCount = await commerceService.GetCountForOwnerIdAsync(ownerId),
                TotalStoresCount = await storeService.GetCountForOwnerIdAsync(ownerId, includeDisabledOptions),
                EnabledStoresCount = await storeService.GetCountForOwnerIdAsync(ownerId),
                TotalItemsCount = await itemService.GetCountForOwnerIdAsync(ownerId, includeDisabledOptions),
                EnabledItemsCount = await itemService.GetCountForOwnerIdAsync(ownerId),
                TotalItemsImagesCount = await itemFileService.GetCountForOwnerIdAsync(ownerId, includeDisabledOptions),
                EnabledItemsImagesCount = await itemFileService.GetCountForOwnerIdAsync(ownerId),
                AggregattedSizeItemsImages = await itemFileService.GetAggregatedSizeForOwnerIdAsync(ownerId, includeDisabledOptions),
                EnabledAggregattedSizeItemsImages = await itemFileService.GetAggregatedSizeForOwnerIdAsync(ownerId),
            };

            return usedPlan;
        }

        public async Task<int> GetMaxTotalCommercesForUserId(Int64 userId)
        {
            var options = new QueryOptions
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
            var options = new QueryOptions
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
            var options = new QueryOptions
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
            var options = new QueryOptions
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
            var options = new QueryOptions
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
            var options = new QueryOptions
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
        
        public async Task<int> GetMaxTotalItemsImagesForCurrentUserId(Int64 userId)
        {
            var options = new QueryOptions
            {
                Join = { { "Plan", "plan" } },
                Filters = {
                    { "UserId", userId },
                    { Op.IsNotNull("plan.MaxEnabledItems") },
                    { Op.GE("ExpirationDate", DateTime.UtcNow) },
                },
                OrderBy = { "plan.MaxTotalItemsImages DESC" },
                Top = 1,
            };

            return (await GetFirstOrDefaultAsync(options))?.Plan?.MaxTotalItemsImages
                ?? (await planService.GetBaseAsync()).MaxTotalItemsImages
                ?? default;
        }

        public async Task<int> GetMaxEnabledItemsImagesForCurrentUserId(Int64 userId)
        {
            var options = new QueryOptions
            {
                Join = { { "Plan", "plan" } },
                Filters = {
                    { "UserId", userId },
                    { Op.IsNotNull("plan.MaxEnabledItems") },
                    { Op.GE("ExpirationDate", DateTime.UtcNow) },
                },
                OrderBy = { "plan.MaxEnabledItemsImages DESC" },
                Top = 1,
            };

            return (await GetFirstOrDefaultAsync(options))?.Plan?.MaxEnabledItemsImages
                ?? (await planService.GetBaseAsync()).MaxEnabledItemsImages
                ?? default;
        }

        public async Task<int> GetMaxAggregattedSizeItemsImagesForCurrentUserId(Int64 userId)
        {
            var options = new QueryOptions
            {
                Join = { { "Plan", "plan" } },
                Filters = {
                    { "UserId", userId },
                    { Op.IsNotNull("plan.MaxEnabledItems") },
                    { Op.GE("ExpirationDate", DateTime.UtcNow) },
                },
                OrderBy = { "plan.MaxAggregattedSizeItemsImages DESC" },
                Top = 1,
            };

            return (await GetFirstOrDefaultAsync(options))?.Plan?.MaxAggregattedSizeItemsImages
                ?? (await planService.GetBaseAsync()).MaxAggregattedSizeItemsImages
                ?? default;
        }

        public async Task<int> GetMaxEnabledAggregattedSizeItemsImagesForCurrentUserId(Int64 userId)
        {
            var options = new QueryOptions
            {
                Join = { { "Plan", "plan" } },
                Filters = {
                    { "UserId", userId },
                    { Op.IsNotNull("plan.MaxEnabledItems") },
                    { Op.GE("ExpirationDate", DateTime.UtcNow) },
                },
                OrderBy = { "plan.MaxEnabledAggregattedSizeItemsImages DESC" },
                Top = 1,
            };

            return (await GetFirstOrDefaultAsync(options))?.Plan?.MaxEnabledAggregattedSizeItemsImages
                ?? (await planService.GetBaseAsync()).MaxEnabledAggregattedSizeItemsImages
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

        public async Task<int> GetMaxTotalItemsImagesForCurrentUser()
        {
            var httpContext = httpContextAccessor.HttpContext
                ?? throw new NoAuthorizationHeaderException();

            var userId = (httpContext.Items["UserId"] as Int64?)
                ?? throw new NoSessionUserDataException();

            if (userId <= 0)
                throw new NoSessionUserDataException();

            return await GetMaxTotalItemsImagesForCurrentUserId(userId);
        }

        public async Task<int> GetMaxEnabledItemsImagesForCurrentUser()
        {
            var httpContext = httpContextAccessor.HttpContext
                ?? throw new NoAuthorizationHeaderException();

            var userId = (httpContext.Items["UserId"] as Int64?)
                ?? throw new NoSessionUserDataException();

            if (userId <= 0)
                throw new NoSessionUserDataException();

            return await GetMaxEnabledItemsImagesForCurrentUserId(userId);
        }

        public async Task<int> GetMaxAggregattedSizeItemsImagesForCurrentUser()
        {
            var httpContext = httpContextAccessor.HttpContext
                ?? throw new NoAuthorizationHeaderException();

            var userId = (httpContext.Items["UserId"] as Int64?)
                ?? throw new NoSessionUserDataException();

            if (userId <= 0)
                throw new NoSessionUserDataException();

            return await GetMaxAggregattedSizeItemsImagesForCurrentUserId(userId);
        }

        public async Task<int> GetMaxEnabledAggregattedSizeItemsImagesForCurrentUser()
        {
            var httpContext = httpContextAccessor.HttpContext
                ?? throw new NoAuthorizationHeaderException();

            var userId = (httpContext.Items["UserId"] as Int64?)
                ?? throw new NoSessionUserDataException();

            if (userId <= 0)
                throw new NoSessionUserDataException();

            return await GetMaxEnabledAggregattedSizeItemsImagesForCurrentUserId(userId);
        }
    }
}

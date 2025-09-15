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
        IServiceProvider serviceProvider
    )
        : ServiceTimestampsIdUuidEnabled<IRepo<UserPlan>, UserPlan>(repo),
            IUserPlanService
    {
        public async Task<Plan> GetSinglePlanForCurrentUserAsync()
        {
            var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
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

            var planService = serviceProvider.GetRequiredService<IPlanService>();

            var plan = userPlan?.Plan ?? await planService.GetBaseAsync();
            return plan;
        }

        public async Task<UsedPlanDTO> GetUsedPlanForCurrentUserAsync()
        {
            var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
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
            var commerceFileService = serviceProvider.GetRequiredService<ICommerceFileService>();
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
                ItemsImagesAggregatedSize = await itemFileService.GetAggregatedSizeForOwnerIdAsync(ownerId, includeDisabledOptions),
                EnabledItemsImagesAggregatedSize = await itemFileService.GetAggregatedSizeForOwnerIdAsync(ownerId),
                TotalCommercesImagesCount = await commerceFileService.GetCountForOwnerIdAsync(ownerId, includeDisabledOptions),
                EnabledCommercesImagesCount = await commerceFileService.GetCountForOwnerIdAsync(ownerId),
                CommercesImagesAggregatedSize = await commerceFileService.GetAggregatedSizeForOwnerIdAsync(ownerId, includeDisabledOptions),
                EnabledCommercesImagesAggregatedSize = await commerceFileService.GetAggregatedSizeForOwnerIdAsync(ownerId),
            };

            return usedPlan;
        }

        public async Task<PlanLimits> GetLimitsForCurrentUserAsync()
        {
            var planService = serviceProvider.GetRequiredService<IPlanService>();
            var plan = await GetSinglePlanForCurrentUserAsync();
            return await planService.GetLimitsForPlanAsync(plan);
        }
    }
}

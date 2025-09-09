using backend_shop.DTO;
using backend_shop.Entities;
using RFService.IServices;

namespace backend_shop.IServices
{
    public interface IUserPlanService
        : IService<UserPlan>,
            IServiceTimestamps<UserPlan>
    {
        Task<Plan> GetSinglePlanForCurrentUserAsync();

        Task<UsedPlanDTO> GetUsedPlanForCurrentUserAsync();

        Task<int> GetMaxTotalCommercesForUserId(Int64 userId);

        Task<int> GetMaxEnabledCommercesForUserId(Int64 userId);

        Task<int> GetMaxTotalStoresForUserId(Int64 userId);

        Task<int> GetMaxEnabledStoresForUserId(Int64 userId);

        Task<int> GetMaxTotalItemsForUserId(Int64 userId);

        Task<int> GetMaxEnabledItemsForUserId(Int64 userId);

        Task<int> GetMaxTotalCommercesForCurrentUser();

        Task<int> GetMaxEnabledCommercesForCurrentUser();

        Task<int> GetMaxTotalStoresForCurrentUser();

        Task<int> GetMaxEnabledStoresForCurrentUser();

        Task<int> GetMaxTotalItemsForCurrentUser();

        Task<int> GetMaxEnabledItemsForCurrentUser();

        Task<int> GetMaxTotalItemsImagesForCurrentUser();

        Task<int> GetMaxEnabledItemsImagesForCurrentUser();

        Task<Int64> GetMaxAggregattedSizeItemsImagesForCurrentUser();

        Task<Int64> GetMaxEnabledAggregattedSizeItemsImagesForCurrentUser();
    }
}

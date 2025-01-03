using backend_shop.Entities;
using RFService.IServices;

namespace backend_shop.IServices
{
    public interface IUserPlanService
        : IService<UserPlan>,
            IServiceTimestamps<UserPlan>
    {
        Task<int> GetMaxTotalBusinessesForUserId(Int64 userId);

        Task<int> GetMaxEnabledBusinessesForUserId(Int64 userId);

        Task<int> GetMaxTotalStoresForUserId(Int64 userId);

        Task<int> GetMaxEnabledStoresForUserId(Int64 userId);

        Task<int> GetMaxTotalItemsForUserId(Int64 userId);

        Task<int> GetMaxEnabledItemsForUserId(Int64 userId);

        Task<int> GetMaxTotalBusinessesForCurrentUser();

        Task<int> GetMaxEnabledBusinessesForCurrentUser();

        Task<int> GetMaxTotalStoresForCurrentUser();

        Task<int> GetMaxEnabledStoresForCurrentUser();

        Task<int> GetMaxTotalItemsForCurrentUser();

        Task<int> GetMaxEnabledItemsForCurrentUser();
    }
}

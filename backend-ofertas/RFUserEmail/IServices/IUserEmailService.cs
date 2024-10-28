using RFService.IService;
using RFUserEmail.Entities;

namespace RFUserEmail.IServices
{
    public interface IUserEmailService : IService<UserEmail>
    {
        Task<UserEmail?> GetSingleOrDefaultAsyncForUserId(Int64 userId);
    }
}

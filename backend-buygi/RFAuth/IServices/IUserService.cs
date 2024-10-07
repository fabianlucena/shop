using RFAuth.Entities;
using RFService.IService;

namespace RFAuth.IServices
{
    public interface IUserService : IService<User>
    {
        Task<User> GetSingleForUsername(string username);
    }
}

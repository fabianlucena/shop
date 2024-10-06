using backend_buygi.Entities;

namespace backend_buygi.IServices
{
    public interface IUserService : IService<User>
    {
        Task<User> GetSingleForUsername(string username);
    }
}

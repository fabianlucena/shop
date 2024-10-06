using backend_buygi.Repo;
using backend_buygi.Entities;
using backend_buygi.IServices;
using backend_buygi.ServicesLib;

namespace backend_buygi.Services
{
    public class UserService : ServiceTimestampsIdUuid<IUserRepo, User>, IUserService
    {
        public UserService(IUserRepo repo)
            : base(repo) { }


        public async Task<User> GetSingleForUsername(string username)
        {
            return await _repo.GetSingle(new GetOptions
            {
                Filters = new { username }
            });
        }
    }
}

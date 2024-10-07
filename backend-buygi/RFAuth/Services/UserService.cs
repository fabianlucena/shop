using RFAuth.IRepo;
using RFAuth.Entities;
using RFAuth.IServices;
using RFService.ServicesLib;
using RFService.RepoLib;

namespace RFAuth.Services
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

using RFAuth.IRepo;
using RFAuth.Entities;
using RFAuth.IServices;
using RFService.ServicesLib;
using RFService.RepoLib;

namespace RFAuth.Services
{
    public class UserService(IUserRepo repo) : ServiceTimestampsIdUuid<IUserRepo, User>(repo), IUserService
    {
        public async Task<User> GetSingleForUsernameAsync(string username)
        {
            return await _repo.GetSingleAsync(new GetOptions
            {
                Filters = new { username }
            });
        }
    }
}

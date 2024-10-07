using RFAuth.IRepo;
using RFAuth.Entities;
using RFAuth.IServices;
using RFService.ServicesLib;
using RFService.RepoLib;

namespace RFAuth.Services
{
    public class PasswordService : ServiceTimestamps<IPasswordRepo, Password>, IPasswordService
    {
        private readonly IUserService _userService;

        public PasswordService(IPasswordRepo repo, IUserService userService)
            :base(repo)
        {
            _userService = userService;
        }

        public string Hash(string rawPassword)
        {
            return BCrypt.Net.BCrypt.HashPassword(rawPassword);
        }

        public bool Verify(string rawPassword, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(rawPassword, hash);
        }

        public bool Verify(string rawPassword, Password password)
        {
            return Verify(rawPassword, password.Hash);
        }

        public async Task<Password> GetSingleForUserId(Int64 userId)
        {
            return await _repo.GetSingle(new GetOptions { Filters = new { userId } });
        }

        public async Task<Password> GetSingleForUser(User user)
        {
            return await GetSingleForUserId(user.Id);
        }
    }
}

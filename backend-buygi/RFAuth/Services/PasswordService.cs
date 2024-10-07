using RFAuth.IRepo;
using RFAuth.Entities;
using RFAuth.IServices;
using RFService.ServicesLib;
using RFService.RepoLib;

namespace RFAuth.Services
{
    public class PasswordService(IPasswordRepo repo) : ServiceTimestamps<IPasswordRepo, Password>(repo), IPasswordService
    {
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

        public async Task<Password> GetSingleForUserIdAsync(Int64 userId)
        {
            return await _repo.GetSingleAsync(new GetOptions { Filters = new { userId } });
        }

        public async Task<Password> GetSingleForUserAsync(User user)
        {
            return await GetSingleForUserIdAsync(user.Id);
        }
    }
}

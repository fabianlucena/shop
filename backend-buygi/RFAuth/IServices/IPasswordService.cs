using RFAuth.Entities;

namespace RFAuth.IServices
{
    public interface IPasswordService
    {
        Task<Password> GetSingleForUserIdAsync(Int64 userId);

        Task<Password> GetSingleForUserAsync(User user);

        string Hash(string password);

        bool Verify(string rawPassword, string hash);

        bool Verify(string rawPassword, Password password);
    }
}

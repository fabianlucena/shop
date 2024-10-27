using RFAuth.DTO;

namespace RFAuth.IServices
{
    public interface ILoginService
    {
        Task<AuthorizationData> LoginAsync(LoginData loginData);
    }
}

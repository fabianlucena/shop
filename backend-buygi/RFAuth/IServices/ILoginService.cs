using RFAuth.DTO;

namespace RFAuth.IServices
{
    public interface ILoginService
    {
        Task<AuthorizationData> Login(LoginData loginData);
    }
}

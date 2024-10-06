using backend_buygi.DTO;

namespace backend_buygi.IServices
{
    public interface ILoginService
    {
        Task<AuthorizationData> login(LoginData loginData);
    }
}

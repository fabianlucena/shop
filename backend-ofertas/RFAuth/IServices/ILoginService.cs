using RFAuth.DTO;
using RFService.IService;

namespace RFAuth.IServices
{
    public interface ILoginService : IServiceAttributes
    {
        Task<LoginResponse> LoginAsync(LoginRequest request);

        Task<LoginResponse> AutoLoginAsync(AutoLoginRequest request);
    }
}

using RFRegister.DTO;

namespace RFRegister.IServices
{
    public interface IRegisterService
    {
        Task RegisterAsync(RegisterData registerData);
    }
}

using RFAuth.Entities;
using RFService.IService;

namespace RFAuth.IServices
{
    public interface IDeviceService : IService<Device>
    {
        Task<Device> GetSingleForTokenOrCreate(string? token);
    }
}

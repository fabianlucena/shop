using backend_buygi.Entities;

namespace backend_buygi.IServices
{
    public interface IDeviceService : IService<Device>
    {
        Task<Device> GetSingleForTokenOrCreate(string token);
    }
}

using RFAuth.Entities;
using RFService.IService;

namespace RFAuth.IServices
{
    public interface ISessionService : IService<Session>
    {
        Task<Session> CreateForUserIdAndDeviceIdAsync(Int64 userId, Int64 deviceId);
        Task<Session> CreateForUserAndDeviceAsync(User user, Device device);
    }
}

using RFAuth.Entities;
using RFService.IService;

namespace RFAuth.IServices
{
    public interface ISessionService : IService<Session>
    {
        Task<Session> CreateForUserIdAndDeviceId(Int64 userId, Int64 deviceId);
        Task<Session> CreateForUserAndDevice(User user, Device device);
    }
}

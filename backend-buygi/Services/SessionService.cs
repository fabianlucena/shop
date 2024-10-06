using backend_buygi.Repo;
using backend_buygi.Entities;
using backend_buygi.Util;
using backend_buygi.Exceptions;
using backend_buygi.IServices;
using backend_buygi.ServicesLib;

namespace backend_buygi.Services
{
    public class SessionService : ServiceTimestampsIdUuid<ISessionRepo, Session>, ISessionService
    {
        public SessionService(ISessionRepo repo)
            :base(repo) { }

        public override async Task<Session> ValidateForCreation(Session data)
        {
            data = await base.ValidateForCreation(data);

            if (data.UserId == 0)
            {
                throw new NullFieldException("UserId");
            }

            if (string.IsNullOrWhiteSpace(data.Token))
            {
                data.Token = Token.GetString(64);
            }

            if (string.IsNullOrWhiteSpace(data.AutoLoginToken))
            {
                data.AutoLoginToken = Token.GetString(64);
            }

            return data;
        }

        public async Task<Session> CreateForUserIdAndDeviceId(Int64 userId, Int64 deviceId)
        {
            var session = new Session
            {
                UserId = userId,
                DeviceId = deviceId,
                Token = "",
                AutoLoginToken = "",
            };

            return await Create(session);
        }

        public async Task<Session> CreateForUserAndDevice(User user, Device device)
        {
            return await CreateForUserIdAndDeviceId(user.Id, device.Id);
        }
    }
}

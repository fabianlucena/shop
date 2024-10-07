using RFAuth.IRepo;
using RFAuth.Entities;
using RFAuth.IServices;
using RFService.ServicesLib;
using RFAuth.Util;
using RFService.Exceptions;

namespace RFAuth.Services
{
    public class SessionService(ISessionRepo repo) : ServiceTimestampsIdUuid<ISessionRepo, Session>(repo), ISessionService
    {
        public override async Task<Session> ValidateForCreationAsync(Session data)
        {
            data = await base.ValidateForCreationAsync(data);

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

        public async Task<Session> CreateForUserIdAndDeviceIdAsync(Int64 userId, Int64 deviceId)
        {
            var session = new Session
            {
                UserId = userId,
                DeviceId = deviceId,
                Token = "",
                AutoLoginToken = "",
            };

            return await CreateAsync(session);
        }

        public async Task<Session> CreateForUserAndDeviceAsync(User user, Device device)
        {
            return await CreateForUserIdAndDeviceIdAsync(user.Id, device.Id);
        }
    }
}

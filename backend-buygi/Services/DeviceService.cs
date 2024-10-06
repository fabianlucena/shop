using backend_buygi.Repo;
using backend_buygi.Entities;
using backend_buygi.Util;
using backend_buygi.IServices;
using backend_buygi.ServicesLib;

namespace backend_buygi.Services
{
    public class DeviceService : ServiceTimestampsIdUuid<IDeviceRepo, Device>, IDeviceService
    {
        public DeviceService(IDeviceRepo deviceRepo)
            :base(deviceRepo) { }

        public override async Task<Device> ValidateForCreation(Device data)
        {
            data = await base.ValidateForCreation(data);

            if (string.IsNullOrEmpty(data.Token))
            {
                data.Token = Token.GetString(64);
            };

            return data;
        }

        public async Task<Device> Create()
        {
            return await Create(new Device { Token = "" }); 
        }

        public async Task<Device?> GetSingleOrNullForToken(string token)
        {
            return await _repo.GetSingleOrNull(new GetOptions
            {
                Filters = new { Token = token }
            });
        }

        public async Task<Device> GetSingleForTokenOrCreate(string token)
        {
            var device = await GetSingleOrNullForToken(token);
            if (device == null)
            {
                device = await Create();
            }

            return device;
        }
    }
}

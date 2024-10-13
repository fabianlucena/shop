using RFAuth.Entities;
using RFAuth.IServices;
using RFService.ServicesLib;
using RFAuth.Util;
using RFService.RepoLib;
using RFAuth.IRepo;

namespace RFAuth.Services
{
    public class DeviceService(IDeviceRepo deviceRepo) : ServiceTimestampsIdUuid<IDeviceRepo, Device>(deviceRepo), IDeviceService
    {
        public override async Task<Device> ValidateForCreationAsync(Device data)
        {
            data = await base.ValidateForCreationAsync(data);

            if (string.IsNullOrEmpty(data.Token))
            {
                data.Token = Token.GetString(64);
            };

            return data;
        }

        public async Task<Device> CreateAsync()
        {
            return await CreateAsync(new Device { Token = "" }); 
        }

        public async Task<Device?> GetSingleOrNullForTokenAsync(string token)
        {
            return await _repo.GetSingleOrDefaultAsync(new GetOptions
            {
                Filters = new { Token = token }
            });
        }

        public async Task<Device> GetSingleForTokenOrCreateAsync(string? token)
        {
            var device = string.IsNullOrWhiteSpace(token)?
                null:
                await GetSingleOrNullForTokenAsync(token);

            device ??= await CreateAsync();

            return device;
        }
    }
}

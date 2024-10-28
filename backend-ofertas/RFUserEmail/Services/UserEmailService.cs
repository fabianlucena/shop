using RFService.EntitiesLib;
using RFService.IRepo;
using RFService.ServicesLib;
using RFUserEmail.Entities;
using RFUserEmail.IServices;

namespace RFUserEmail.Services
{
    public class UserEmailService(IRepo<UserEmail> repo) : ServiceTimestampsIdUuid<IRepo<UserEmail>, UserEmail>(repo), IUserEmailService
    {
        public override async Task<UserEmail> ValidateForCreationAsync(UserEmail data)
        {
            data = await base.ValidateForCreationAsync(data);
            data.IsVerified ??= false;

            return data;
        }
    }
}

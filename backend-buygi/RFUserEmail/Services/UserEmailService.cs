using RFService.IRepo;
using RFService.ServicesLib;
using RFUserEmail.Entities;
using RFUserEmail.IServices;

namespace RFUserEmail.Services
{
    public class UserEmailService(IRepo<UserEmail> repo) : ServiceTimestampsIdUuid<IRepo<UserEmail>, UserEmail>(repo), IUserEmailService
    {
    }
}

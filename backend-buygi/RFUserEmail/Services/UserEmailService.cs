using RFService.ServicesLib;
using RFUserEmail.Entities;
using RFUserEmail.IRepo;
using RFUserEmail.IServices;

namespace RFUserEmail.Services
{
    public class UserEmailService(IUserEmailRepo repo) : ServiceTimestampsIdUuid<IUserEmailRepo, UserEmail>(repo), IUserEmailService
    {
    }
}

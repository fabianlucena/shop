using RFAuth.Entities;
using RFAuth.IServices;
using RFService.ServicesLib;
using RFService.IRepo;

namespace RFAuth.Services
{
    public class UserTypeService(IRepo<UserType> repo) : ServiceTimestampsIdUuidEnabledNameTitleTranslatable<IRepo<UserType>, UserType>(repo), IUserTypeService
    {
    }
}

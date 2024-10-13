using RFAuth.IRepo;
using RFAuth.Entities;
using RFAuth.IServices;
using RFService.ServicesLib;

namespace RFAuth.Services
{
    public class UserTypeService(IUserTypeRepo repo) : ServiceTimestampsIdUuidEnabledNameTitleTranslatable<IUserTypeRepo, UserType>(repo), IUserTypeService
    {
    }
}

using RFService.EntitiesLib;
using RFService.IRepo;

namespace RFService.ServicesLib
{
    public abstract class ServiceTimestampsIdUuidEnabledNameTitleTranslatable<Repo, Entity>(Repo repo) : ServiceTimestampsIdUuidEnabledName<Repo, Entity>(repo)
        where Repo : IRepo<Entity>
        where Entity : EntityTimestampsIdUuidEnabledNameTitleTranslatable
    {
    }
}

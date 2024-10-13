using RFService.EntitiesLib;
using RFService.IRepo;

namespace RFService.ServicesLib
{
    public abstract class ServiceTimestampsIdUuidEnabled<Repo, Entity>(Repo repo) : ServiceTimestampsIdUuid<Repo, Entity>(repo)
        where Repo : IRepo<Entity>
        where Entity : EntityTimestampsIdUuidEnabled
    {
    }
}

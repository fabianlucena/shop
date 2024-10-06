using backend_buygi.Exceptions;
using backend_buygi.Repo;
using backend_buygi.EntitiesLib;

namespace backend_buygi.ServicesLib
{
    public abstract class ServiceTimestampsId<Repo, Entity> : ServiceTimestamps<Repo, Entity>
        where Repo : IRepo<Entity>
        where Entity : EntityTimestampsId
    {
        public ServiceTimestampsId(Repo repo)
            :base(repo) { }

        public override async Task<Entity> ValidateForCreation(Entity data)
        {
            data = await base.ValidateForCreation(data);

            if (data.Id != 0)
            {
                throw new ForbidenIdForCreationException();
            }

            return data;
        }
    }
}
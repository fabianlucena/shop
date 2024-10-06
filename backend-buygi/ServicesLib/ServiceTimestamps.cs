using backend_buygi.Repo;
using backend_buygi.EntitiesLib;

namespace backend_buygi.ServicesLib
{
    public abstract class ServiceTimestamps<Repo, Entity> : ServiceBase<Repo, Entity>
        where Repo : IRepo<Entity>
        where Entity : EntityTimestamps
    {
        public ServiceTimestamps(Repo repo)
            : base(repo) { }

        public override async Task<Entity> ValidateForCreation(Entity data)
        {
            data = await base.ValidateForCreation(data);

            data.CreatedAt = DateTime.UtcNow;
            data.UpdatedAt = DateTime.UtcNow;

            return data;
        }
    }
}
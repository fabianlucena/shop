using RFService.EntitiesLib;
using RFService.IRepo;
using RFService.RepoLib;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RFService.ServicesLib
{
    public abstract class ServiceTimestamps<Repo, Entity>(Repo repo) : ServiceBase<Repo, Entity>(repo)
        where Repo : IRepo<Entity>
        where Entity : EntityTimestamps
    {
        public override async Task<Entity> ValidateForCreationAsync(Entity data)
        {
            data = await base.ValidateForCreationAsync(data);

            data.CreatedAt = DateTime.UtcNow;
            data.UpdatedAt = DateTime.UtcNow;

            return data;
        }

        public override Task<IEnumerable<Entity>> GetListAsync(GetOptions? options)
        {
            return base.GetListAsync(options);
        }
    }
}
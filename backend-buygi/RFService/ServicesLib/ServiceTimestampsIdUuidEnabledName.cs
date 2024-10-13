using RFService.EntitiesLib;
using RFService.IRepo;
using RFService.RepoLib;

namespace RFService.ServicesLib
{
    public abstract class ServiceTimestampsIdUuidEnabledName<Repo, Entity>(Repo repo) : ServiceTimestampsIdUuidEnabled<Repo, Entity>(repo)
        where Repo : IRepo<Entity>
        where Entity : EntityTimestampsIdUuidEnabledName
    {
        public async Task<Entity> GetSingleForNameAsync(string Name)
        {
            return await _repo.GetSingleAsync(new GetOptions
            {
                Filters = new { Name }
            });
        }

        public async Task<Entity?> GetSingleOrDefaultForNameAsync(string Name)
        {
            return await _repo.GetSingleOrDefaultAsync(new GetOptions
            {
                Filters = new { Name }
            });
        }
    }
}

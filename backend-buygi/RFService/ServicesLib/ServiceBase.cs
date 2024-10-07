using RFService.EntitiesLib;
using RFService.RepoLib;
using RFService.IRepo;

namespace RFService.ServicesLib
{
    public abstract class ServiceBase<Repo, Entity>(Repo repo)
        where Repo : IRepo<Entity>
        where Entity : EntityTimestamps
    {
        protected readonly Repo _repo = repo;

        public virtual async Task<Entity> ValidateForCreationAsync(Entity data)
        {
            return await Task.Run(() => data);
        }

        public virtual async Task<Entity> CreateAsync(Entity data)
        {
            data = await ValidateForCreationAsync(data);
            return await _repo.InsertAsync(data);
        }

        public virtual Task<IEnumerable<Entity>> GetListAsync(GetOptions? options)
        {
            return _repo.GetListAsync(options);
        }
    }
}
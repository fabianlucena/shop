using backend_buygi.Repo;
using backend_buygi.EntitiesLib;

namespace backend_buygi.ServicesLib
{
    public abstract class ServiceBase<Repo, Entity>
        where Repo : IRepo<Entity>
        where Entity : EntityTimestamps
    {
        protected readonly Repo _repo;

        public ServiceBase(Repo repo)
        {
            _repo = repo;
        }

        public virtual async Task<Entity> ValidateForCreation(Entity data)
        {
            return await Task.Run(() => data);
        }

        public virtual async Task<Entity> Create(Entity data)
        {
            data = await ValidateForCreation(data);
            return await _repo.Insert(data);
        }

        public virtual Task<IEnumerable<Entity>> GetList(GetOptions? options)
        {
            return _repo.GetList(options);
        }
    }
}
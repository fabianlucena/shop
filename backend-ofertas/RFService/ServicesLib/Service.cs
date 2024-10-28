using RFService.EntitiesLib;
using RFService.RepoLib;
using RFService.IRepo;

namespace RFService.ServicesLib
{
    public abstract class Service<Repo, Entity>(Repo repo)
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

        public virtual Task<IEnumerable<Entity>> GetListAsync(GetOptions options)
        {
            return _repo.GetListAsync(options);
        }

        public virtual Task<Entity> GetSingleAsync(GetOptions options)
        {
            return _repo.GetSingleAsync(options);
        }

        public virtual Task<Entity?> GetSingleOrDefaultAsync(GetOptions options)
        {
            return _repo.GetSingleOrDefaultAsync(options);
        }

        public virtual Task<Entity?> GetFirstOrDefaultAsync(GetOptions options)
        {
            return _repo.GetFirstOrDefaultAsync(options);
        }

        public virtual GetOptions SanitizeForAutoGet(GetOptions options)
        {
            return options;
        }

        public virtual Task<Entity?> AutoGetFirstOrDefaultAsync(GetOptions options)
        {
            return GetFirstOrDefaultAsync(SanitizeForAutoGet(options));
        }

        public virtual Task<Entity?> AutoGetFirstOrDefaultAsync(Entity data)
        {
            var filters = new Dictionary<string, object?>();
            var entityType = typeof(Entity);
            var properties = entityType.GetProperties();
            foreach (var pInfo in properties)
            {
                var pType = pInfo.PropertyType;
                if (pType.IsClass && pType.Name != "String")
                    continue;

                filters[pInfo.Name] = pInfo.GetValue(data);
            }

            return AutoGetFirstOrDefaultAsync(new GetOptions { Filters = filters });
        }

        public virtual async Task<Entity> GetOrCreateAsync(Entity data)
        {
            var res = await AutoGetFirstOrDefaultAsync(data);
            if (res != null)
                return res;

            return await CreateAsync(data);
        }

        public virtual Task CreateIfNotExistsAsync(Entity data)
        {
            return GetOrCreateAsync(data);
        }

        public virtual Task<int> UpdateAsync(object data, GetOptions options)
        {
            return _repo.UpdateAsync(data, options);
        }
    }
}
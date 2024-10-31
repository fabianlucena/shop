using RFService.Repo;

namespace RFService.IService
{
    public interface IService<Entity>
        where Entity : class
    {
        Task<Entity> ValidateForCreationAsync(Entity data);

        Task<Entity> CreateAsync(Entity data);

        Task<IEnumerable<Entity>> GetListAsync(GetOptions options);

        Task<Entity> GetSingleAsync(GetOptions options);

        Task<Entity?> GetSingleOrDefaultAsync(GetOptions options);

        Task<Entity?> GetFirstOrDefaultAsync(GetOptions options);

        GetOptions SanitizeForAutoGet(GetOptions options);

        Task<Entity?> AutoGetFirstOrDefaultAsync(GetOptions options);

        Task<Entity?> AutoGetFirstOrDefaultAsync(Entity data);

        Task<Entity> GetOrCreateAsync(Entity data);

        Task CreateIfNotExistsAsync(Entity data);

        Task<int> UpdateAsync(object data, GetOptions options);

        Task<int> UpdateForIdAsync(object data, Int64 id, GetOptions? options = null);
    }
}

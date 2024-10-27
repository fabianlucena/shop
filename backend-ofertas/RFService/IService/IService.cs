using RFService.RepoLib;

namespace RFService.IService
{
    public interface IService<Entity>
        where Entity : class
    {
        Task<Entity> ValidateForCreationAsync(Entity data);

        Task<Entity> CreateAsync(Entity data);

        Task<IEnumerable<Entity>> GetListAsync(GetOptions? options = null);

        Task<Entity?> GetSingleOrDefaultAsync(GetOptions? options = null);

        Task<Entity?> GetFirstOrDefaultAsync(GetOptions? options = null);

        GetOptions SanitizeForAutoGet(GetOptions options);

        Task<Entity?> AutoGetFirstOrDefaultAsync(GetOptions options);

        Task<Entity?> AutoGetFirstOrDefaultAsync(Entity data);

        Task<Entity> GetOrCreateAsync(Entity data);

        Task CreateIfNotExistsAsync(Entity data);
    }
}

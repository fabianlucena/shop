using RFService.RepoLib;

namespace RFService.IRepo
{
    public interface IRepo<Entity>
    {
        Task<Entity> InsertAsync(Entity data);
        Task<Entity> GetSingleAsync(GetOptions? options = null);
        Task<Entity?> GetSingleOrDefaultAsync(GetOptions? options = null);
        Task<IEnumerable<Entity>> GetListAsync(GetOptions? options = null);
    }
}

using RFService.RepoLib;

namespace RFService.IRepo
{
    public interface IRepo<Entity>
    {
        Task<Entity> Insert(Entity data);
        Task<Entity> GetSingle(GetOptions? options = null);
        Task<Entity?> GetSingleOrNull(GetOptions? options = null);
        Task<IEnumerable<Entity>> GetList(GetOptions? options = null);
    }
}

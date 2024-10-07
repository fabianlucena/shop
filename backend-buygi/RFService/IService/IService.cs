using RFService.RepoLib;

namespace RFService.IService
{
    public interface IService<Entity>
        where Entity : class
    {
        Task<Entity> CreateAsync(Entity data);

        Task<IEnumerable<Entity>> GetListAsync(GetOptions? options = null);
    }
}

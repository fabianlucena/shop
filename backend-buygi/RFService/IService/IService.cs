using RFService.RepoLib;

namespace RFService.IService
{
    public interface IService<Entity>
        where Entity : class
    {
        Task<Entity> Create(Entity data);

        Task<IEnumerable<Entity>> GetList(GetOptions? options = null);
    }
}

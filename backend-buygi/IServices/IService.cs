using backend_buygi.Repo;

namespace backend_buygi.IServices
{
    public interface IService<Entity>
        where Entity : class
    {
        Task<Entity> Create(Entity data);

        Task<IEnumerable<Entity>> GetList(GetOptions? options = null);
    }
}

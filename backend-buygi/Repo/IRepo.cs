namespace backend_buygi.Repo
{
    public interface IRepo<Entity>
    {
        Task<Entity> Insert(Entity data);
        Task<Entity> GetSingle(GetOptions? options = null);
        Task<Entity?> GetSingleOrNull(GetOptions? options = null);
        Task<IEnumerable<Entity>> GetList(GetOptions? options = null);
    }
}

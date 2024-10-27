namespace RFService.IService
{
    public interface IServiceName<Entity>
        where Entity : class
    {
        Task<Entity> GetSingleForNameAsync(string name);

        Task<Entity?> GetSingleOrDefaultForNameAsync(string name);
    }
}

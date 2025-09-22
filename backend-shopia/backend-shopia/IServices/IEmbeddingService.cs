namespace backend_shopia.IServices
{
    public interface IEmbeddingService
    {
        Task<float[]> GetAsync(string data);
    }
}

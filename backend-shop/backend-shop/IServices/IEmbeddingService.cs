namespace backend_shop.IServices
{
    public interface IEmbeddingService
    {
        Task<float[]> GetAsync(string data);
    }
}

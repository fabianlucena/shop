using backend_shop.IServices;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace backend_shop.Services
{
    public class EmbeddingService(
        string url,
        string apiKey,
        string model
    )
        : IEmbeddingService

    {
        public async Task<float[]> GetAsync(string data)
        {
            using var http = new HttpClient { BaseAddress = new Uri(url) };
            if (!string.IsNullOrEmpty(apiKey))
                http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            var payload = new
            {
                model,
                prompt = data,
            };

            using var req = new HttpRequestMessage(HttpMethod.Post, "embeddings")
            {
                Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json")
            };

            using var res = await http.SendAsync(req);
            res.EnsureSuccessStatusCode();

            var text = await res.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(text);
            var emb = doc.RootElement.GetProperty("embedding");
            var arr = new float[emb.GetArrayLength()];
            int i = 0;
            foreach (var n in emb.EnumerateArray())
                arr[i++] = n.GetSingle();

            return arr;
        }
    }
}

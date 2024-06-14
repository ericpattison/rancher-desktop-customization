using System.Text.Json;
using Models;

namespace Client;

public interface ISvc2 {
    Task<Order?> GetOrderById(int id);
}

public class Svc2 : ISvc2
{
    private const string BaseUrl = "http://svc2.default.svc.cluster.local:38080";
    private readonly HttpClient httpClient;

    public Svc2(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<Order?> GetOrderById(int id) {
        var response = await httpClient.GetAsync($"{BaseUrl}/orders/{id}");
        var result = await response.Content.ReadAsStreamAsync();

        return await JsonSerializer.DeserializeAsync<Order>(result);
    }

}
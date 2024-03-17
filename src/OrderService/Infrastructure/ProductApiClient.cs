using System.Text.Json;
using OrderService.Contracts.Consumed;

namespace OrderService.Infrastructure;

public class ProductApiClient
{
    private readonly HttpClient _httpClient;

    public ProductApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ProductData> GetProductAsync(int id)
    {
        var response = await _httpClient.GetAsync($"api/products/{id}");
        var strContent = await response.Content.ReadAsStringAsync();
        response.EnsureSuccessStatusCode();
        var data =   await response.Content.ReadFromJsonAsync<ProductData>() ?? new ProductData();
        return data;
    }
}
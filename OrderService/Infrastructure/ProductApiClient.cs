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
        var response = await _httpClient.GetAsync($"products/{id}");
        response.EnsureSuccessStatusCode();
        var data = await response.Content.ReadFromJsonAsync<ProductData>() ?? new ProductData();
        return data;
    }
}
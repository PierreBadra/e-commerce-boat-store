using Newtonsoft.Json;
using pbadraH60Customer.DTO;

namespace pbadraH60Customer.DAL;

public class ProductRepository : IProductRepository
{
    private readonly HttpClient _httpClient;

    public ProductRepository(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Accept", "applcation/json");
        var baseUrl = Environment.GetEnvironmentVariable("API_BASE_URL") ?? "http://localhost:5202";
        _httpClient.BaseAddress = new Uri($"{baseUrl}");
    }
    
    public async Task<List<ProductCustomerDisplayDTO>> GetProducts()
    {
        List<ProductCustomerDisplayDTO> products = null;
        string endpoint = $"/api/CustomerProducts";
        HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

        if (!response.IsSuccessStatusCode)
        {
            return new List<ProductCustomerDisplayDTO>();
        }

        string responseData = await response.Content.ReadAsStringAsync();
        products = JsonConvert.DeserializeObject<List<ProductCustomerDisplayDTO>>(responseData);

        return products;
    }

    public async Task<ProductCustomerDisplayDTO> GetProduct(int id)
    {
        ProductCustomerDisplayDTO product = null;
        string endpoint = $"/api/CustomerProducts/Details/{id}";

        HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        string responseData = await response.Content.ReadAsStringAsync();
        product = JsonConvert.DeserializeObject<ProductCustomerDisplayDTO>(responseData);

        return product;
    }
}
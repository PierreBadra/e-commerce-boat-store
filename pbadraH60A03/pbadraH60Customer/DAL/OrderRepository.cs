using pbadraH60Customer.DTO;
using System.Text;
using Newtonsoft.Json;

namespace pbadraH60Customer.DAL;

public class OrderRepository : IOrderRepository
{
    private readonly HttpClient _httpClient;

    public OrderRepository(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        var baseUrl = Environment.GetEnvironmentVariable("API_BASE_URL") ?? "http://localhost:5202";
        _httpClient.BaseAddress = new Uri($"{baseUrl}");
    }
    
    public async Task<bool> Create(OrderDTO newOrder)
    {
        string endpoint = $"/api/Order";
        string jsonContent = JsonConvert.SerializeObject(newOrder);

        StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        HttpResponseMessage response = await _httpClient.PostAsync(endpoint, content);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> Update(OrderDTO updatedOrder)
    {
        string endpoint = $"/api/Order/{updatedOrder.OrderId}";
        string jsonContent = JsonConvert.SerializeObject(updatedOrder);

        StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        HttpResponseMessage response = await _httpClient.PutAsync(endpoint, content);

        return response.IsSuccessStatusCode;
    }

    public async Task<OrderDTO> Find(int orderId)
    {
        OrderDTO order = null;
        string endpoint = $"/api/Order/{orderId}";

        HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        string responseData = await response.Content.ReadAsStringAsync();
        order = JsonConvert.DeserializeObject<OrderDTO>(responseData);

        return order;
    }

    public async Task<List<OrderDTO>> FindByCustomerName(string customerName)
    {
        List<OrderDTO> orders = null;
        var endpoint = $"/api/Order/Customer/{customerName}";
        
        HttpResponseMessage response = await _httpClient.GetAsync(endpoint);
        
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }
        
        string responseData = await response.Content.ReadAsStringAsync();
        orders = JsonConvert.DeserializeObject<List<OrderDTO>>(responseData);

        return orders;
    }

    public async Task<List<OrderDTO>> FindByCustomerId(int customerId)
    {
        List<OrderDTO> orders = null;
        var endpoint = $"/api/Order/Customer/CustomerId/{customerId}";
        
        HttpResponseMessage response = await _httpClient.GetAsync(endpoint);
        
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }
        
        string responseData = await response.Content.ReadAsStringAsync();
        orders = JsonConvert.DeserializeObject<List<OrderDTO>>(responseData);

        return orders;
    }
}
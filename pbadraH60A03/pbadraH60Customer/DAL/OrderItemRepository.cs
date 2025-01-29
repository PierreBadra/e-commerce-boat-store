using System.Text;
using Newtonsoft.Json;
using pbadraH60Customer.Models;

namespace pbadraH60Customer.DAL;

public class OrderItemRepository : IOrderItemRepository
{
    private readonly HttpClient _httpClient;

    public OrderItemRepository(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        var baseUrl = Environment.GetEnvironmentVariable("API_BASE_URL") ?? "http://localhost:5202";
        _httpClient.BaseAddress = new Uri($"{baseUrl}");
    }
    
    public async Task<bool> Create(List<OrderItemDTO> orderItems)
    {
        string endpoint = $"/api/OrderItem";

        foreach (var orderItem in orderItems)
        {
            string jsonContent = JsonConvert.SerializeObject(orderItem);

            StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync(endpoint, content);

            if (!response.IsSuccessStatusCode) return false;
        }
        return true;
    }

    public async Task<bool> Update(OrderItemDTO orderItem)
    {
        string endpoint = $"/api/OrderItem/{orderItem.OrderItemId}";
        string jsonContent = JsonConvert.SerializeObject(orderItem);

        StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        HttpResponseMessage response = await _httpClient.PutAsync(endpoint, content);
        
        return response.IsSuccessStatusCode;
    }

    public async Task<OrderItemDTO> Find(int id)
    {
        OrderItemDTO orderItem = null;
        string endpoint = $"/api/OrderItem/{id}";

        HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        string responseData = await response.Content.ReadAsStringAsync();
        orderItem = JsonConvert.DeserializeObject<OrderItemDTO>(responseData);

        return orderItem;
    }

    public async Task<List<OrderItemDTO>> FindByOrderId(int orderId)
    {
        List<OrderItemDTO> orderItems = null;
        string endpoint = $"/api/OrderItem/Order/{orderId}";

        HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        string responseData = await response.Content.ReadAsStringAsync();
        orderItems = JsonConvert.DeserializeObject<List<OrderItemDTO>>(responseData);

        return orderItems;
    }
}
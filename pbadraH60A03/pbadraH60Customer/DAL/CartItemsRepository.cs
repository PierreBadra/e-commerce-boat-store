using System.Text;
using Newtonsoft.Json;
using pbadraH60Customer.DTO;
using pbadraH60Customer.Models;

namespace pbadraH60Customer.DAL;

public class CartItemsRepository : ICartItemsRepository
{
    private readonly HttpClient _httpClient;

    public CartItemsRepository(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        var baseUrl = Environment.GetEnvironmentVariable("API_BASE_URL") ?? "http://localhost:5202";
        _httpClient.BaseAddress = new Uri($"{baseUrl}");
    }
    public async Task<bool> Create(CartItem cartItem)
    {
        string endpoint = $"/api/CartItems";
        string jsonContent = JsonConvert.SerializeObject(cartItem);

        StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        HttpResponseMessage response = await _httpClient.PostAsync(endpoint, content);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> Update(CartItem cartItem)
    {
        string endpoint = $"/api/CartItems/{cartItem.CartItemId}";
        string jsonContent = JsonConvert.SerializeObject(cartItem);

        StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        HttpResponseMessage response = await _httpClient.PutAsync(endpoint, content);
        
        return response.IsSuccessStatusCode;
    }

    public Task<CartItem> Find(int id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Delete(CartItem cartItem)
    {
        throw new NotImplementedException();
    }
    
    public async Task<List<CartItemDTO>> FindByShoppingCartId(int shoppingCartId)
    {
        List<CartItemDTO> cartItems = null;
        string endpoint = $"/api/CartItems/shoppingCart/{shoppingCartId}";
        HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

        if (!response.IsSuccessStatusCode)
        {
            return new List<CartItemDTO>();
        }

        string responseData = await response.Content.ReadAsStringAsync();
        cartItems = JsonConvert.DeserializeObject<List<CartItemDTO>>(responseData);

        return cartItems;
    }
}
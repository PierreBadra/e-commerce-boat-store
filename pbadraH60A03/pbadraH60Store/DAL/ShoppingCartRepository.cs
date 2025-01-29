using System.Text;
using Newtonsoft.Json;
using pbadraH60A01.Models;

namespace pbadraH60Store.DAL;

public class ShoppingCartRepository : IShoppingCartRepository<ShoppingCart>
{
    private readonly HttpClient _httpClient;

    public ShoppingCartRepository(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        var baseUrl = Environment.GetEnvironmentVariable("API_BASE_URL") ?? "http://localhost:5202";
        _httpClient.BaseAddress = new Uri($"{baseUrl}");
    }
    
    public async Task<bool> Create(ShoppingCart newShoppingCart)
    {
        string endpoint = $"/api/ShoppingCart";
        string jsonContent = JsonConvert.SerializeObject(newShoppingCart);

        StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        HttpResponseMessage response = await _httpClient.PostAsync(endpoint, content);

        return !response.IsSuccessStatusCode;
    }

    public async Task<bool> Update(ShoppingCart updatedShoppingCart)
    {
        string endpoint = $"/api/ShoppingCart/{updatedShoppingCart.CartId}";
        string jsonContent = JsonConvert.SerializeObject(updatedShoppingCart);

        StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        HttpResponseMessage response = await _httpClient.PutAsync(endpoint, content);

        return !response.IsSuccessStatusCode;
    }

    public async Task<ShoppingCart> Find(int shoppingCartId)
    {
        ShoppingCart shoppingCart = null;
        string endpoint = $"/api/ShoppingCart/{shoppingCartId}";

        HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        string responseData = await response.Content.ReadAsStringAsync();
        shoppingCart = JsonConvert.DeserializeObject<ShoppingCart>(responseData);

        return shoppingCart;
    }

    public async Task<bool> Delete(ShoppingCart deletedShoppingCart)
    {
        try
        {
            string endpoint = $"/api/ShoppingCart/{deletedShoppingCart.CartId}";
            HttpResponseMessage response = await _httpClient.DeleteAsync(endpoint);
            return !response.IsSuccessStatusCode;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using pbadraH60A01.Models;
using System.Text;

namespace pbadraH60A01.Services
{
    public class ProductRepository : IProductRepository
    {
        private readonly HttpClient _httpClient;

        public ProductRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            var baseUrl = Environment.GetEnvironmentVariable("API_BASE_URL") ?? "http://localhost:5202";
            _httpClient.BaseAddress = new Uri($"{baseUrl}");
        }

        public async Task<bool> Create(Product newProduct)
        {
            string endpoint = $"/api/Products";
            string jsonContent = JsonConvert.SerializeObject(newProduct);

            StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync(endpoint, content);

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            return true;
        }

        public async Task<List<Product>> Read()
        {
            List<Product> products = null;
            string endpoint = $"/api/Products";
            HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

            if (!response.IsSuccessStatusCode)
            {
                return new List<Product>();
            }

            string responseData = await response.Content.ReadAsStringAsync();
            products = JsonConvert.DeserializeObject<List<Product>>(responseData);

            return products;
        }

        public async Task<bool> Update(Product updatedProduct)
        {
            string endpoint = $"/api/Products/{updatedProduct.ProductId}";
            string jsonContent = JsonConvert.SerializeObject(updatedProduct);

            StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PutAsync(endpoint, content);

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> Delete(Product deletedProduct)
        {
            try
            {
                string endpoint = $"/api/Products/{deletedProduct.ProductId}";
                HttpResponseMessage reponse = await _httpClient.DeleteAsync(endpoint);

                if (!reponse.IsSuccessStatusCode)
                    return false;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<Product> Find(int productId)
        {
            Product product = null;
            string endpoint = $"/api/Products/{productId}";

            HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            string responseData = await response.Content.ReadAsStringAsync();
            product = JsonConvert.DeserializeObject<Product>(responseData);

            return product;
        }

        public async Task<CategoryProductsViewModel> ReadByCategory(int categoryId)
        {
            CategoryProductsViewModel products = null;
            string endpoint = $"/api/Products/Category/{categoryId}";
            HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

            if (!response.IsSuccessStatusCode)
            {
                return products;
            }

            string responseData = await response.Content.ReadAsStringAsync();
            products = JsonConvert.DeserializeObject<CategoryProductsViewModel>(responseData);

            return products;
        }

        public async Task<Product> FindWithCategory(int productId)
        {
            Product product = null;
            string endpoint = $"/api/Products/{productId}";
            HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

            if (!response.IsSuccessStatusCode)
            {
                return product;
            }

            string responseData = await response.Content.ReadAsStringAsync();
            product = JsonConvert.DeserializeObject<Product>(responseData);

            return product;
        }

        public async Task<bool> UpdateStock(Product updatedProduct)
        {
            string endpoint = $"/api/Products/ProductStock/{updatedProduct.ProductId}";
            string jsonContent = JsonConvert.SerializeObject(updatedProduct);

            StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PatchAsync(endpoint, content);

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> UpdateSellPrice(Product updatedProduct)
        {
            string endpoint = $"/api/Products/ProductSellPrice/{updatedProduct.ProductId}";
            string jsonContent = JsonConvert.SerializeObject(updatedProduct);

            StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PatchAsync(endpoint, content);

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> UpdateBuyPrice(Product updatedProduct)
        {
            string endpoint = $"/api/Products/ProductBuyPrice/{updatedProduct.ProductId}";
            string jsonContent = JsonConvert.SerializeObject(updatedProduct);

            StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PatchAsync(endpoint, content);

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            return true;
        }
    }
}

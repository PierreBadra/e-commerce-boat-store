using Newtonsoft.Json;
using pbadraH60A01.Models;
using System.Text;

namespace pbadraH60A01.Services
{
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly HttpClient _httpClient;

        public ProductCategoryRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Accept", "applcation/json");
            var baseUrl = Environment.GetEnvironmentVariable("API_BASE_URL") ?? "http://localhost:5202";
            _httpClient.BaseAddress = new Uri($"{baseUrl}");
        }

        public async Task<string> Create(ProductCategory newProductCategory)
        {
            string categoriesEndPoint = $"/api/ProductCategories";
            string jsonContent = JsonConvert.SerializeObject(newProductCategory);

            StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync(categoriesEndPoint, content);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                return errorMessage;
            }
            return null;
        }

        public async Task<List<ProductCategory>> Read()
        {
            List<ProductCategory> productCategories = null;
            string endpoint = $"/api/ProductCategories";

            HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

            if (!response.IsSuccessStatusCode)
            {
                return new List<ProductCategory>();
            }

            string responseData = await response.Content.ReadAsStringAsync();
            productCategories = JsonConvert.DeserializeObject<List<ProductCategory>>(responseData);

            return productCategories;
        }

        public async Task<string> Update(ProductCategory updatedCategory)
        {
            string endpoint = $"/api/ProductCategories/{updatedCategory.CategoryId}";
            string jsonContent = JsonConvert.SerializeObject(updatedCategory);

            StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PutAsync(endpoint, content);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                return errorMessage;
            }

            return "";
        }

        public async Task<string> Delete(ProductCategory deletedCategory)
        {
            string endpoint = $"/api/ProductCategories/{deletedCategory.CategoryId}";

            HttpResponseMessage response = await _httpClient.DeleteAsync(endpoint);

            if (!response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
                
            }

            return "";

        }

        public async Task<ProductCategory> Find(int categoryId)
        {
            ProductCategory category = null;
            string endpoint = $"/api/ProductCategories/{categoryId}";

            if (categoryId == null)
                return null;

            HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();

                category = JsonConvert.DeserializeObject<ProductCategory>(responseData);
                return category;
            }

            return null;
        }
    }
}

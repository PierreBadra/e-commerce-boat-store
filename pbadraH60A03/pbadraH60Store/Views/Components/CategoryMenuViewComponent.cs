using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using pbadraH60A01.Models;
using pbadraH60A01.Services;

namespace pbadraH60A01.Views.Components
{
    public class CategoryMenuViewComponent : ViewComponent
    {
        private readonly HttpClient _httpClient;

        public CategoryMenuViewComponent(HttpClient httpClient)
        {
            _httpClient = httpClient;
            var baseUrl = Environment.GetEnvironmentVariable("API_BASE_URL") ?? "http://localhost:5202";
            _httpClient.BaseAddress = new Uri($"{baseUrl}");
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            List<ProductCategory> productCategories = null;
            string endpoint = $"/api/ProductCategories";

            HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

            string responseData = await response.Content.ReadAsStringAsync();
            productCategories = JsonConvert.DeserializeObject<List<ProductCategory>>(responseData);



            return View("/Views/Shared/CategoryMenu.cshtml", productCategories.Select(x => new CategoryViewModel { Id = x.CategoryId, Name = x.ProdCat}));
        }
    }
}

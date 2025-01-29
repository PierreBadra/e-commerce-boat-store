using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using pbadraH60Customer.Models;
using System.Text;
using pbadraH60Customer.DAL;

namespace pbadraH60Customer.DAL
{
    public class CustomerRepository : ICustomerRepository
    {
        private HttpClient _httpClient;

        public CustomerRepository(HttpClient httpClient) {
            _httpClient = httpClient;
            var baseUrl = Environment.GetEnvironmentVariable("API_BASE_URL") ?? "http://localhost:5202";
            _httpClient.BaseAddress = new Uri($"{baseUrl}");
        }

        public async Task<bool> Create(Customer newCustomer)
        {
            string categoriesEndPoint = $"/api/Customers";
            string jsonContent = JsonConvert.SerializeObject(newCustomer);

            StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync(categoriesEndPoint, content);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> Delete(Customer deletedCustomer)
        {
            string customersEndPoint = $"/api/Customers/{deletedCustomer.CustomerId}";
            HttpResponseMessage response = await _httpClient.DeleteAsync(customersEndPoint);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }

        public async Task<Customer> Find(int customerId)
        {
            Customer customer = null;
            string endpoint = $"/api/Customers/{customerId}";

            if (customerId == null)
                return null;

            HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();

                customer = JsonConvert.DeserializeObject<Customer>(responseData);
                return customer;
            }

            return null;
        }


        public async Task<Customer> FindByUserId(string userId)
        {
            Customer customer = null;
            string endpoint = $"/api/Customers/UserId/{userId}";

            if (userId == null)
                return null;

            HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();

                customer = JsonConvert.DeserializeObject<Customer>(responseData);
                return customer;
            }

            return null;
        }


        public async Task<List<Customer>> Read()
        {
            List<Customer> customers = null;
            string endpoint = $"/api/Customers";

            HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

            if (!response.IsSuccessStatusCode)
            {
                return new List<Customer>();
            }

            string responseData = await response.Content.ReadAsStringAsync();
            customers = JsonConvert.DeserializeObject<List<Customer>>(responseData);

            return customers;
        }

        public async Task<bool> Update(Customer updatedCustomer)
        {
            string endpoint = $"/api/Customers/{updatedCustomer.CustomerId}";
            string jsonContent = JsonConvert.SerializeObject(updatedCustomer);

            StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PutAsync(endpoint, content);

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            return true;
        }
    }
}

using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;

namespace pbadraH60Customer.Models;

public class CreditCardAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is not string cardNumber)
        {
            return new ValidationResult("Credit card is required");
        }

        var creditCard = new { CardNumber = cardNumber };

        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            var baseUrl = Environment.GetEnvironmentVariable("API_BASE_URL") ?? "http://localhost:5202";
            client.BaseAddress = new Uri($"{baseUrl}");
            
            var content = new StringContent(JsonSerializer.Serialize(creditCard), Encoding.UTF8, "application/json");
            var response = client.PostAsync("/api/Checkout/CheckCreditCard", content).GetAwaiter().GetResult();;

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                return new ValidationResult(errorMessage);
            }
        }

        return ValidationResult.Success;
    }
}
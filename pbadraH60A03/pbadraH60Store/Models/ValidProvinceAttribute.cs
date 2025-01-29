using System.ComponentModel.DataAnnotations;

namespace pbadraH60A01.Models
{
    public class ValidProvinceAttribute : ValidationAttribute
    {
        private readonly string[] _validProvinces = { "QC", "ON", "NB", "MB" };

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // Allow the province field to be null or empty (optional)
            if (value == null || string.IsNullOrEmpty(value as string))
            {
                return ValidationResult.Success; // Field is optional
            }

            string? province = value as string;

            // Check if the entered value is one of the valid provinces
            if (!_validProvinces.Contains(province))
            {
                return new ValidationResult($"The province must be one of the following: {string.Join(", ", _validProvinces)}.");
            }

            return ValidationResult.Success;
        }
    }
}

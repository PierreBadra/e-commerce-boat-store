using System.ComponentModel.DataAnnotations;

namespace pbadraH60Services.DTO
{
    public class CustomerDTO
    {
        public int CustomerId { get; set; }

        [RegularExpression(@"^[a-zA-Z' -]+$", ErrorMessage = "The first name must contain only letters, apostrophes, dashes, and spaces.")]
        public string? FirstName { get; set; } = null!;

        [RegularExpression(@"^[a-zA-Z' -]+$", ErrorMessage = "The last name must contain only letters, apostrophes, dashes, and spaces.")]
        public string? LastName { get; set; } = null!;

        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email format.")]
        public string? Email { get; set; } = null!;

        [RegularExpression(@"^(\+?\d{1,3}[- ]?)?(\(?\d{3}\)?[- ]?)?\d{3}[- ]?\d{4}$|^$", ErrorMessage = "Invalid phone number format.")]
        public string? PhoneNumber { get; set; } = null!;

        public string? Province { get; set; }

        public string? Password { get; set; }

        public string? CreditCard { get; set; } = null!;
    }
}

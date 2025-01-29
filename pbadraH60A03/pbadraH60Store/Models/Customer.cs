using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace pbadraH60A01.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    [RegularExpression(@"^[a-zA-Z' -]+$", ErrorMessage = "The first name must contain only letters, apostrophes, dashes, and spaces.")]
    public string? FirstName { get; set; } = null!;

    [RegularExpression(@"^[a-zA-Z' -]+$", ErrorMessage = "The last name must contain only letters, apostrophes, dashes, and spaces.")]
    public string? LastName { get; set; } = null!;

    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email format.")]
    [Required(ErrorMessage = "The email is required.")]
    [UniqueEmail]
    public string? Email { get; set; }

    [RegularExpression(@"^(\+?\d{1,3}[- ]?)?(\(?\d{3}\)?[- ]?)?\d{3}[- ]?\d{4}$|^$", ErrorMessage = "Invalid phone number format.")]
    public string? PhoneNumber { get; set; } = null!;

    [ValidProvince(ErrorMessage = "The province must either be Quebec, Ontario, New Brunswick, or Manitoba.")]
    public string? Province { get; set; }

    [Required(ErrorMessage = "The password is required.")]
    [StringLength(100, ErrorMessage = "{0} length must be between {2} and {1} characters", MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [Compare("Password", ErrorMessage = "Password confirmation does not match.")]
    [NotMapped]
    public string? ConfirmPassword { get; set; }

    [MinLength(16, ErrorMessage = "The credit card number must be exactly 16 digits long.")]
    [MaxLength(16, ErrorMessage = "The credit card number must not exceed 16 digits.")]
    [RegularExpression(@"^\d+$", ErrorMessage = "The credit card number must be composed of only positive integer numbers.")]
    public string? CreditCard { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    [JsonIgnore]
    public virtual ShoppingCart? ShoppingCart { get; set; }

    public string? UserId { get; set; }
}

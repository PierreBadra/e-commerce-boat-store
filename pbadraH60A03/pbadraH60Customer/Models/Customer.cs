using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using pbadraH60A01.Models;
using pbadraH60Customer.Models;

namespace pbadraH60Customer.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    [RegularExpression(@"^[a-zA-Z' -]+$", ErrorMessage = "The first name must contain only letters, apostrophes, dashes, and spaces.")]
    public string? FirstName { get; set; } = null!;
    
    [RegularExpression(@"^[a-zA-Z' -]+$", ErrorMessage = "The last name must contain only letters, apostrophes, dashes, and spaces.")]
    public string? LastName { get; set; } = null!;

    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email format.")]
    [Required(ErrorMessage = "The email is required.")]
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
    
    [CreditCard]
    public string? CreditCard { get; set; } = null!;
    
    [NotMapped]
    [Required(ErrorMessage = "The address is required.")]
    public string Address { get; set; }
    
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    
    public virtual ShoppingCart? ShoppingCart { get; set; }
    public string? UserId { get; set; }
}

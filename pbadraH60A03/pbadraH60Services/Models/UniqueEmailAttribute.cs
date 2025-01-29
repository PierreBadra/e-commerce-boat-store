using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using pbadraH60Services.Models;

public class UniqueEmailAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        // Resolve UserManager from the service provider
        var userManager = validationContext.GetService(typeof(UserManager<IdentityUser>)) as UserManager<IdentityUser>;

        if (userManager == null)
        {
            return new ValidationResult("UserManager service not available.");
        }

        string newEmail = value?.ToString() ?? string.Empty;

        // Get the existing customer instance
        var customerInstance = (Customer)validationContext.ObjectInstance;

        // Fetch the existing user based on the CustomerId (or any identifier)
        var existingUser = userManager.FindByIdAsync(customerInstance.UserId).Result;

        // Fetch all users
        var allUsers = userManager.Users.ToList();

        // If the existing user is found, remove it from the list to prevent false duplicates
        if (existingUser != null)
        {
            allUsers.Remove(existingUser);
        }

        // Check if the new email already exists among the other users
        bool emailExists = allUsers.Any(u => u.Email.Equals(newEmail, StringComparison.OrdinalIgnoreCase));

        if (emailExists)
        {
            return new ValidationResult("The email is already in use.");
        }

        return ValidationResult.Success;
    }
}

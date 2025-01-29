using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using pbadraH60Customer.Areas.Identity.Pages.Account.Manage;
using pbadraH60Customer.Models;

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
        
        var customerInstance = (IndexModel.InputModel)validationContext.ObjectInstance;
        
        var existingUser = userManager.FindByIdAsync(customerInstance.UserId).Result;
        
        var allUsers = userManager.Users.ToList();
        
        if (existingUser != null)
        {
            allUsers.Remove(existingUser);
        }
        
        bool emailExists = allUsers.Any(u => u.Email.Equals(newEmail, StringComparison.OrdinalIgnoreCase));

        if (emailExists)
        {
            return new ValidationResult("The email is already in use.");
        }

        return ValidationResult.Success;
    }
}

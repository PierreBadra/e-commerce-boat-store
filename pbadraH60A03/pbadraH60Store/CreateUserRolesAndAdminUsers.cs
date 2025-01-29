using Microsoft.AspNetCore.Identity;
using pbadraH60A01.Models;

namespace pbadraH60Store
{
    public class CreateUserRolesAndAdminUsers
    {
        public static async Task Execute(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var dbContext = scope.ServiceProvider.GetRequiredService<H60assignmentDbPbadraContext>();

                string[] roleNames = { "Customer", "Clerk", "Manager" };
                IdentityResult roleResult;

                foreach (var roleName in roleNames)
                {
                    var roleExist = await roleManager.RoleExistsAsync(roleName);
                    if (!roleExist)
                    {
                        roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                    }
                }

                var managerUser = await userManager.FindByEmailAsync("manager@gmail.com");
                if (managerUser == null)
                {
                    managerUser = new IdentityUser()
                    {
                        UserName = "manager@gmail.com",
                        Email = "manager@gmail.com",
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(managerUser, "Password-123");
                    await userManager.AddToRoleAsync(managerUser, "Manager");
                }

                var clerkUser = await userManager.FindByEmailAsync("clerk@gmail.com");
                if (clerkUser == null)
                {
                    clerkUser = new IdentityUser()
                    {
                        UserName = "clerk@gmail.com",
                        Email = "clerk@gmail.com",
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(clerkUser, "Password-123");
                    await userManager.AddToRoleAsync(clerkUser, "Clerk");
                }

                var customerUser = await userManager.FindByEmailAsync("customer@gmail.com");
                if (customerUser == null)
                {
                    customerUser = new IdentityUser()
                    {
                        UserName = "customer@gmail.com",
                        Email = "customer@gmail.com",
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(customerUser, "Password-123");
                    await userManager.AddToRoleAsync(customerUser, "Customer");

                    var userId = await userManager.GetUserIdAsync(customerUser);
                    var customer = new Customer()
                    {
                        Email = customerUser.Email,
                        UserId = userId
                    };
                    dbContext.Customers.Add(customer);
                    dbContext.SaveChanges();
                    
                    var createdCustomer = dbContext.Customers.FirstOrDefault(c => c.UserId == userId);

                    var shoppingCart = new ShoppingCart()
                    {
                        CustomerId = createdCustomer.CustomerId,
                        DateCreated = DateTime.Now
                    };
                    dbContext.ShoppingCarts.Add(shoppingCart);
                    dbContext.SaveChanges();
                }
            }
        }
    }
}

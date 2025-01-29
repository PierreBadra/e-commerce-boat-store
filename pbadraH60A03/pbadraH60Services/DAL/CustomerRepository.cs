using Microsoft.AspNetCore.Identity;
using pbadraH60Services.Models;

namespace pbadraH60Services.DAL
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly H60assignment2DbPbadraContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CustomerRepository(H60assignment2DbPbadraContext context, UserManager<IdentityUser> userManager) {
            _context = context;
            _userManager = userManager;
        }

        public async Task<bool> Create(Customer newCustomer)
        {
            try
            {
                var user = Activator.CreateInstance<IdentityUser>();
                await _userManager.SetPhoneNumberAsync(user,newCustomer.PhoneNumber);
                await _userManager.SetUserNameAsync(user, newCustomer.Email);
                await _userManager.SetEmailAsync(user, newCustomer.Email);
                user.EmailConfirmed = true;
                var result = await _userManager.CreateAsync(user, newCustomer.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Customer");
                    newCustomer.UserId = user.Id;
                    newCustomer.Password = null;
                    _context.Customers.Add(newCustomer);
                    _context.SaveChanges();
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> Delete(Customer deletedCustomer)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(deletedCustomer.UserId);
                await _userManager.DeleteAsync(user);
                return true;

            } catch (Exception)
            {
                return false;
            }
        }

        public Customer Find(int customerId)
        {
            return _context.Customers.Find(customerId);
        }

        public Customer FindByUserId(string userId)
        {
            return _context.Customers.FirstOrDefault(x => x.UserId == userId);
        }

        public List<Customer> Read()
        {
            return _context.Customers.OrderBy(x => x.FirstName).ThenBy(x => x.LastName).ToList();
        }

        public async Task<bool> Update(Customer updatedCustomer)
        {
            try
            {
                var existingCustomer = await _context.Customers.FindAsync(updatedCustomer.CustomerId);
                var user = await _userManager.FindByIdAsync(updatedCustomer.UserId);
                user.UserName = updatedCustomer.Email;
                user.Email = updatedCustomer.Email;
                user.PhoneNumber = updatedCustomer.PhoneNumber;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    existingCustomer.FirstName = updatedCustomer.FirstName;
                    existingCustomer.LastName = updatedCustomer.LastName;
                    existingCustomer.Email = updatedCustomer.Email;
                    existingCustomer.PhoneNumber = updatedCustomer.PhoneNumber;
                    existingCustomer.Province = updatedCustomer.Province;
                    existingCustomer.CreditCard = updatedCustomer.CreditCard;
                    await _context.SaveChangesAsync();
                    return true;
                }              
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

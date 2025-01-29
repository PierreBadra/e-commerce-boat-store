using pbadraH60Services.Services;
using Microsoft.AspNetCore.Mvc;
using pbadraH60Services.DAL;
using pbadraH60Services.Models;
using pbadraH60Services.DTO;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;


namespace pbadraH60Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : Controller
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomersController(ICustomerRepository customerRepository, UserManager<IdentityUser> userManager)
        {
            _customerRepository = customerRepository;
        }

        [HttpGet]
        public ActionResult<List<Customer>> GetCustomers([FromQuery] string? search) {
            if (string.IsNullOrEmpty(search))
                return _customerRepository.Read();


            return _customerRepository.Read().Where(c =>
                (c.FirstName?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (c.LastName?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                c.Email.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                (c.PhoneNumber?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (c.Province?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false)).ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<Customer> GetCustomer(int id)
        {
            var customer = _customerRepository.Find(id);
            if (customer == null)
                return NotFound();

            return customer;
        }

        [HttpGet("UserId/{userId}")]
        public ActionResult<Customer> GetCustomerByUserId(string userId)
        {
            var customer = _customerRepository.FindByUserId(userId);
            if (customer == null)
                return NotFound();

            return customer;
        }

        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(CustomerDTO newCustomerDTO)
        {

            if (!ModelState.IsValid)
            {

                return Conflict(ModelState.Values);
            }

            var cust = new Customer()
            {
                CustomerId = newCustomerDTO.CustomerId,
                FirstName = newCustomerDTO.FirstName,
                LastName = newCustomerDTO.LastName,
                Email = newCustomerDTO.Email,
                PhoneNumber = newCustomerDTO.PhoneNumber,
                Province = newCustomerDTO.Province,
                Password = newCustomerDTO.Password,
                CreditCard = newCustomerDTO.CreditCard,
            };

            if (!await _customerRepository.Create(cust))
                return Conflict("An error occured");

            return CreatedAtAction(nameof(PostCustomer), new { id = cust.CustomerId }, cust);

        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Customer>> PutCustomer(int id, CustomerDTO updatedCustomerDTO)
        {
            if (!ModelState.IsValid)
            {
                return Conflict(ModelState.Values);
            }

            var cust = new Customer()
            {
                CustomerId = updatedCustomerDTO.CustomerId,
                FirstName = updatedCustomerDTO.FirstName,
                LastName = updatedCustomerDTO.LastName,
                Email = updatedCustomerDTO.Email,
                PhoneNumber = updatedCustomerDTO.PhoneNumber,
                Province = updatedCustomerDTO.Province,
                Password = updatedCustomerDTO.Password,
                CreditCard = updatedCustomerDTO.CreditCard,
                UserId = _customerRepository.Find(updatedCustomerDTO.CustomerId).UserId
            };

            if (!await _customerRepository.Update(cust))
                return Conflict("An error occured");

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Customer>> DeleteCustomer(int id)
        {
            var deletedCustomer = _customerRepository.Find(id);

            if (deletedCustomer == null)
            {
                return NotFound();
            }

            string errorMessage = "";

            if (deletedCustomer.ShoppingCart != null && deletedCustomer.Orders.Any())
            {
                errorMessage = "One or many shopping carts orders are associated with this customer. To successfully delete a customer, you must cancel all shopping cart orders from the customer beforehand.";
                return Conflict(errorMessage);
            }

            if (!await _customerRepository.Delete(deletedCustomer))
            {
                errorMessage = "An error occured";
                return Conflict(errorMessage);
            }


            return Ok();
        }
    }
}

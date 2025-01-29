using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pbadraH60A01.DAL;
using pbadraH60A01.Models;
using pbadraH60Store.DAL;

namespace pbadraH60Store.Controllers
{
    [Authorize(Roles = "Clerk,Manager")]
    public class CustomersController : Controller
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IShoppingCartRepository<ShoppingCart> _shoppingCartRepository;

        public CustomersController(ICustomerRepository customerRepository, IShoppingCartRepository<ShoppingCart> shoppingCartRepoitory)
        {
            _customerRepository = customerRepository;
            _shoppingCartRepository = shoppingCartRepoitory;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _customerRepository.Read());
        }

        [HttpGet]
        public IActionResult Create() {
            ViewData["Provinces"] = new Dictionary<string, string>()
            {
                { "QC", "Quebec" },
                { "ON", "Ontario" },
                { "NB", "New Brunswick" },
                { "MB", "Manitoba" }
            };
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Customer newCustomer)
        {   
            if (ModelState.IsValid)
            {
                if (await _customerRepository.Create(newCustomer))
                {
                    var shoppingCart = new ShoppingCart()
                    {
                        CustomerId = newCustomer.CustomerId,
                        DateCreated = DateTime.Now
                    };
                    await _shoppingCartRepository.Create(shoppingCart);
                    
                    TempData["SuccessMessage"] = "Customer created successfully!";
                    return RedirectToAction("Index");
                }
            }

            ViewData["Provinces"] = new Dictionary<string, string>()
                {
                    { "QC", "Quebec" },
                    { "ON", "Ontario" },
                    { "NB", "New Brunswick" },
                    { "MB", "Manitoba" }
                };
            return View(newCustomer);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var customer = await _customerRepository.Find(id);
            if (customer == null)
            {
                return NotFound();
            }
            ViewData["Provinces"] = new Dictionary<string, string>()
            {
                { "QC", "Quebec" },
                { "ON", "Ontario" },
                { "NB", "New Brunswick" },
                { "MB", "Manitoba" }
            };
            return View(customer);
        }

        public async Task<IActionResult> Edit(Customer updatedCustomer)
        {
            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");
            if (ModelState.IsValid)
            {
                if (await _customerRepository.Update(updatedCustomer))
                {
                    TempData["SuccessMessage"] = "Customer updated successfully!";
                    return RedirectToAction("Index");
                }
            }

            ViewData["Provinces"] = new Dictionary<string, string>()
            {
                { "QC", "Quebec" },
                { "ON", "Ontario" },
                { "NB", "New Brunswick" },
                { "MB", "Manitoba" }
            };

            return View(updatedCustomer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var existingCustomer = await _customerRepository.Find(id);
            if (existingCustomer == null)
            {
                return NotFound();
            }

            if (!await _customerRepository.Delete(existingCustomer))
            {
                TempData["ErrorMessage"] = "Could not delete customer.";
                return RedirectToAction("Index");

            }

            TempData["SuccessMessage"] = "Customer deleted successfully!";
            return RedirectToAction("Index");
        }
    }
}

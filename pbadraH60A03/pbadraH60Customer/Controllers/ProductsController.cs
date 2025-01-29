using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using pbadraH60Customer.DAL;
using pbadraH60Customer.Models;

namespace pbadraH60Customer.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IShoppingCartRepository<ShoppingCart> _shoppingCartRepository;
        private readonly ICartItemsRepository _cartItemRepository;
        private readonly INotyfService _notyfService;
        private readonly ICustomerRepository _customerRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public ProductsController(
            IProductRepository productRepo, 
            IShoppingCartRepository<ShoppingCart> shoppingCartRepo, 
            ICartItemsRepository cartItemRepo,
            INotyfService notyfService,
            ICustomerRepository customerRepository,
            UserManager<IdentityUser> userManager)
        {
            _productRepository = productRepo;
            _shoppingCartRepository = shoppingCartRepo;
            _cartItemRepository = cartItemRepo;
            _notyfService = notyfService;
            _customerRepository = customerRepository;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _productRepository.GetProducts());
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id) { 
            
            var product = await _productRepository.GetProduct(id);
            if (product == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            var customer = await _customerRepository.FindByUserId(userId);
            
            if (customer == null)
                return View(product);
            
            var shoppingCart = await _shoppingCartRepository.FindByCustomerId(customer.CustomerId);

            if (shoppingCart != null)
            {
                var cartId = shoppingCart.CartId;
                ViewData["shoppingCartId"] = cartId;
            }
            
            return View(product);
        }
        
        [Authorize(Roles = "Customer")]
        [HttpPost]
        public async Task<IActionResult> Details([FromForm] int productId, int? quantity, decimal price, int cartId)
        {
            var product = await _productRepository.GetProduct(productId);
            ViewData["shoppingCartId"] = cartId;
            
            if (quantity == null)
            {
                _notyfService.Error("The quantity must be specified");
                return View(product);
            }
            
            var cartItem = new CartItem() { ProductId = productId, Quantity = quantity.Value, Price = price, CartId = cartId };

            if (quantity <= 0)
            {
                _notyfService.Error("The quantity cannot be negative or zero");
                return View(product);
            }
            
            if (product.Stock - quantity < 0)
            {
                _notyfService.Error("The quantity exceeds the stock");
                return View(product);
            }

            if (!await _cartItemRepository.Create(cartItem))
            {
                _notyfService.Error("An error occured while adding item to cart");
                return View(product);
            }
            
            _notyfService.Success("Item successfully added to cart");
            return RedirectToAction("Index");
        }
    }
}

using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using pbadraH60Customer.DAL;
using pbadraH60Customer.DTO;
using pbadraH60Customer.Models;

namespace pbadraH60Customer.Controllers;

[Authorize(Roles = "Customer")]
public class ShoppingCartController : Controller
{
    private readonly IShoppingCartRepository<ShoppingCart> _shoppingCartRepository;
    private readonly ICartItemsRepository _cartItemRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly INotyfService _notyfService;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderItemRepository _orderItemRepository;


    public ShoppingCartController(
        ICustomerRepository customerRepository,
        IShoppingCartRepository<ShoppingCart> shoppingCartRepository, 
        ICartItemsRepository cartItemRepository,  
        UserManager<IdentityUser> userManager,
        INotyfService notyfService,
        IOrderRepository orderRepository,
        IOrderItemRepository orderItemRepository)
    {
        _shoppingCartRepository = shoppingCartRepository;
        _cartItemRepository = cartItemRepository;
        _customerRepository = customerRepository;
        _userManager = userManager;
        _notyfService = notyfService;
        _orderRepository = orderRepository;
        _orderItemRepository = orderItemRepository;
    }
    
    public async Task<IActionResult> Index()
    {
        var customer = await _customerRepository.FindByUserId(_userManager.GetUserId(User));

        var shoppingCart = await _shoppingCartRepository.FindByCustomerId(customer.CustomerId);
        
        var cartItems = await _cartItemRepository.FindByShoppingCartId(shoppingCart.CartId);
        
        return View(cartItems);
    }

    [HttpGet]
    public async Task<IActionResult> Checkout()
    {
        var customer = await _customerRepository.FindByUserId(_userManager.GetUserId(User));
        var shoppingCart = await _shoppingCartRepository.FindByCustomerId(customer.CustomerId);
        var cartItems = await _cartItemRepository.FindByShoppingCartId(shoppingCart.CartId);

        if (shoppingCart == null)
        {
            _notyfService.Error("No shopping cart found");
            return RedirectToAction("Index", "Products");
        }

        if (cartItems.Count == 0)
        {
            _notyfService.Error("No cart items found");
            return RedirectToAction("Index", "Products");
        }
        var viewModel = new CustomerCartItemsViewModel()
        {
            Customer = customer,
            CartItems = cartItems
        };
            
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Checkout(CustomerCartItemsViewModel viewModel, decimal total, decimal tax)
    {
        ModelState.Remove("Customer.Password");
        ModelState.Remove("Customer.ConfirmPassword");
        
        if (!ModelState.IsValid) return View(viewModel);

        var now = DateTime.Now;
        var order = new OrderDTO()
        {
            CustomerId = viewModel.Customer.CustomerId,
            DateCreated = now,
            Total = total,
            Taxes = tax,
            CustomerName = $"{viewModel.Customer.FirstName} {viewModel.Customer.LastName}".Trim(),
            CustomerEmail = viewModel.Customer.Email,
        };
        if (await _orderRepository.Create(order))
        {
            var customerOrders = await _orderRepository.FindByCustomerId(viewModel.Customer.CustomerId);
            var customerOrder = customerOrders.FirstOrDefault(o => o.DateCreated == now);
            
            List<OrderItemDTO> orderItems = new List<OrderItemDTO>();

            foreach (var cartItem in viewModel.CartItems)
            {
                orderItems.Add(new OrderItemDTO() { ProductId = cartItem.ProductId, Quantity = cartItem.Quantity, OrderId = customerOrder.OrderId, Price = cartItem.Price});
            }
            
            await _orderItemRepository.Create(orderItems);
            _notyfService.Success("Your order has been placed and will be delivered shortly :)");
        }
        else
        {
            _notyfService.Error("An error has occured while processing your order");
        }
        
        return RedirectToAction("Index", "Products");
    }
}
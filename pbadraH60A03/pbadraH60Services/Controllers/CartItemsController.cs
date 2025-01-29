using Microsoft.AspNetCore.Mvc;
using pbadraH60Services.DAL;
using pbadraH60Services.DTO;
using pbadraH60Services.Models;

namespace pbadraH60Services.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CartItemsController : Controller
{
    private readonly ICartItemRepository<CartItem> _cartItemRepository;

    public CartItemsController(ICartItemRepository<CartItem> cartItemRepository)
    {
        _cartItemRepository = cartItemRepository;
    }

    [HttpGet]
    public async Task<List<CartItemDTO>> GetCartItems()
    {
        var cartItems = await _cartItemRepository.Read();
        var cartItemsDTO = cartItems.Select(x => new CartItemDTO(x));
        return cartItemsDTO.ToList();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CartItemDTO>> GetCartItem(int id)
    {
        var shoppingCart = await _cartItemRepository.Find(id);
        if (shoppingCart == null)
            return NotFound();

        var cartItemDTO = new CartItemDTO()
        {
            CartItemId = shoppingCart.CartItemId,
            CartId = shoppingCart.CartId,
            ProductId = shoppingCart.ProductId,
            Quantity = shoppingCart.Quantity,
            Price = shoppingCart.Price,
        };

        return Ok(cartItemDTO);
    }

    [HttpGet("ShoppingCart/{id}")]
    public async Task<ActionResult<List<CartItemDTO>>> GetCartItems(int id)
    {
        var cartItems = await _cartItemRepository.FindByCartId(id);
        
        if (cartItems == null) return NotFound();
        
        var cartItemDtoS = cartItems.Select(x => new CartItemDTO(x)).ToList();
        
        return cartItemDtoS;
    }

    [HttpPost]
    public async Task<ActionResult> PostCartItem(CartItemDTO cartItemDTO)
    {
        if (!ModelState.IsValid)
            return Conflict();

        var cartItem = new CartItem()
        {
            CartId = cartItemDTO.CartId,
            CartItemId = cartItemDTO.CartItemId,
            ProductId = cartItemDTO.ProductId,
            Quantity = cartItemDTO.Quantity,
            Price = cartItemDTO.Price,
        };

        if (!await _cartItemRepository.Create(cartItem))
            return Conflict("An error occurred");

        return CreatedAtAction(nameof(PostCartItem), new { id = cartItem.CartItemId }, cartItemDTO);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> PutCartItem(int id, CartItemDTO cartItemDTO)
    {
        if (!ModelState.IsValid)
            return Conflict();

        var cartItem = new CartItem()
        {
            CartId = cartItemDTO.CartId,
            CartItemId = cartItemDTO.CartItemId,
            ProductId = cartItemDTO.ProductId,
            Quantity = cartItemDTO.Quantity,
            Price = cartItemDTO.Price,
        };

        if (!await _cartItemRepository.Update(cartItem))
            return Conflict("An error occurred");

        return Ok();
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult<CartItemDTO>> DeleteShoppingCart(int id)
    {
        var shoppingCart = await _cartItemRepository.Find(id);
        if (shoppingCart == null)
            return NotFound();

        if (!await _cartItemRepository.Delete(id))
            return Conflict("Cannot delete shopping cart containing cart items");

        return Ok();
    }
}
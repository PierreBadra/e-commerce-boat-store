using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using pbadraH60Services.DAL;
using pbadraH60Services.DTO;
using pbadraH60Services.Models;

namespace pbadraH60Services.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ShoppingCartController : Controller
{
    private readonly IShoppingCartRepository<ShoppingCart> _shoppingCartRepository;

    public ShoppingCartController(IShoppingCartRepository<ShoppingCart> shoppingCartRepository)
    {
        _shoppingCartRepository = shoppingCartRepository;
    }

    [HttpGet]
    public async Task<List<ShoppingCartDTO>> GetShoppingCarts()
    {
        var shoppingCart = await _shoppingCartRepository.Read();
        var shoppingCartDTO = shoppingCart.Select(x => new ShoppingCartDTO(x));
        return shoppingCartDTO.ToList();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ShoppingCartDTO>> GetShoppingCart(int id)
    {
        var shoppingCart = await _shoppingCartRepository.Find(id);
        if (shoppingCart == null)
            return NotFound();

        var shoppingCartDTO = new ShoppingCart()
        {
            CartId = shoppingCart.CartId,
            CustomerId = shoppingCart.CustomerId,
            DateCreated = shoppingCart.DateCreated,
        };

        return Ok(shoppingCartDTO);
    }

    [HttpGet("/api/ShoppingCart/Customer/{id}")]
    public async Task<ActionResult<ShoppingCartDTO>> GetCustomerShoppingCart(int id)
    {
        var shoppingCart = await _shoppingCartRepository.FindByCustomerId(id);
        
        if (shoppingCart == null) return NotFound();
        
        var shoppingCartDTO = new ShoppingCart()
        {
            CartId = shoppingCart.CartId,
            CustomerId = shoppingCart.CustomerId,
            DateCreated = shoppingCart.DateCreated,
            CartItems = shoppingCart.CartItems
        };
        
        return Ok(shoppingCartDTO);
    } 
    
    [HttpPost]
    public async Task<ActionResult> PostShoppingCart(ShoppingCartDTO shoppingCartDTO)
    {
        if (!ModelState.IsValid)
            return Conflict();

        var shoppingCart = new ShoppingCart()
        {
            CartId = shoppingCartDTO.CartId,
            CustomerId = shoppingCartDTO.CustomerId,
            DateCreated = shoppingCartDTO.DateCreated,
        };

        if (!await _shoppingCartRepository.Create(shoppingCart))
            return Conflict("An error occurred during creation");

        return CreatedAtAction(nameof(PostShoppingCart), new { id = shoppingCart.CartId }, shoppingCartDTO);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ShoppingCartDTO>> DeleteShoppingCart(int id)
    {
        var shoppingCart = await _shoppingCartRepository.Find(id);
        if (shoppingCart == null)
            return NotFound();

        if (!await _shoppingCartRepository.Delete(id))
            return Conflict("Cannot delete shopping cart containing cart items");

        return Ok();
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult<ShoppingCart>> PutShoppingCart(int id, ShoppingCartDTO updatedShoppingCartDTO)
    {
        if (!ModelState.IsValid)
        {
            return Conflict();
        }

        var shoppingCart = new ShoppingCart()
        {
            CartId = updatedShoppingCartDTO.CartId,
            CustomerId = updatedShoppingCartDTO.CustomerId,
            DateCreated = updatedShoppingCartDTO.DateCreated,
        };

        if (!await _shoppingCartRepository.Update(shoppingCart))
            return Conflict("An error occured");

        return Ok();
    }
}
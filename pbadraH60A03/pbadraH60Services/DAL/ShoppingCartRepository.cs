using Microsoft.EntityFrameworkCore;
using pbadraH60Services.Models;

namespace pbadraH60Services.DAL;

public class ShoppingCartRepository : IShoppingCartRepository<ShoppingCart>
{
    private readonly H60assignment2DbPbadraContext _context;

    public ShoppingCartRepository(H60assignment2DbPbadraContext context)
    {
        _context = context;
    }

    public async Task<bool> Create(ShoppingCart newShoppingCart)
    {
        try
        {
            await _context.ShoppingCarts.AddAsync(newShoppingCart);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<List<ShoppingCart>> Read()
    {
        return await _context.ShoppingCarts.ToListAsync() ?? new List<ShoppingCart>();
    }

    public async Task<bool> Update(ShoppingCart updatedShoppingCart)
    {
        var existingShoppingCart =
            await _context.ShoppingCarts.FirstOrDefaultAsync(s => s.CartId == updatedShoppingCart.CartId);

        if (existingShoppingCart == null) return false;

        try
        {
            existingShoppingCart.CartItems = new List<CartItem>();
            existingShoppingCart.DateCreated = updatedShoppingCart.DateCreated;
            existingShoppingCart.CustomerId = updatedShoppingCart.CustomerId;
            _context.ShoppingCarts.Update(existingShoppingCart);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> Delete(int id)
    {
        var shoppingCart = await _context.ShoppingCarts.FirstOrDefaultAsync(s => s.CartId == id);

        if (shoppingCart.CartItems.Any())
            return false;

        _context.ShoppingCarts.Remove(shoppingCart);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<ShoppingCart> Find(int id)
    {
        return await _context.ShoppingCarts.FirstOrDefaultAsync(s => s.CartId == id);
    }

    public async Task<ShoppingCart> FindByCustomerId(int customerId)
    {
        return await _context.ShoppingCarts.FirstOrDefaultAsync(s => s.CustomerId == customerId);
    }
}
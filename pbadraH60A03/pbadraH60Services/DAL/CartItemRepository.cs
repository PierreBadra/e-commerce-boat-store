using System.Xml;
using Microsoft.EntityFrameworkCore;
using pbadraH60Services.DTO;
using pbadraH60Services.Models;

namespace pbadraH60Services.DAL;

public class CartItemRepository : ICartItemRepository<CartItem>
{
    private readonly H60assignment2DbPbadraContext _context;

    public CartItemRepository(H60assignment2DbPbadraContext context)
    {
        _context = context;
    }

    public async Task<bool> Create(CartItem newCartItem)
    {
        var product = await _context.Products.FindAsync(newCartItem.ProductId);

        if (product == null) return false;

        product.Stock -= newCartItem.Quantity;
        if (product.Stock < 0)
        {
            return false;
        }

        _context.Products.Update(product);

        try
        {
            var existingCartItems = await FindByCartId(newCartItem.CartId);
            var existingCartItem = existingCartItems.FirstOrDefault(cItem => cItem.ProductId == newCartItem.ProductId);

            if (existingCartItem != null)
            {
                existingCartItem.Quantity += newCartItem.Quantity;
                _context.CartItems.Update(existingCartItem);
            }
            else
            {
                await _context.CartItems.AddAsync(newCartItem);
            }
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<List<CartItem>> Read()
    {
        return await _context.CartItems.Include(cI => cI.Product).ThenInclude(p => p.ProdCat).ToListAsync() ?? new List<CartItem>();
    }

    public async Task<bool> Update(CartItem updatedCartItem)
    {
        var existingCartItem =
            await _context.CartItems.FirstOrDefaultAsync(s => s.CartItemId == updatedCartItem.CartItemId);
        
        var product = await _context.Products.FindAsync(existingCartItem.ProductId);

        if (product == null || existingCartItem == null)
        {
            return false;
        }

        product.Stock += (existingCartItem.Quantity - updatedCartItem.Quantity);
        if (product.Stock < 0)
        {
            return false;
        }

        _context.Products.Update(product);

        try
        {
            existingCartItem.CartId = updatedCartItem.CartId;
            existingCartItem.Price = updatedCartItem.Price;
            existingCartItem.Quantity = updatedCartItem.Quantity;
            existingCartItem.ProductId = updatedCartItem.ProductId;
            _context.CartItems.Update(existingCartItem);
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
        var cartItem = await _context.CartItems.FirstOrDefaultAsync(s => s.CartItemId == id);
        var product = await _context.Products.FindAsync(cartItem.ProductId);
        if (cartItem == null || product == null)
            return false;
        
        product.Stock += cartItem.Quantity;
        _context.Products.Update(product);
        
        try
        {
            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<CartItem> Find(int id)
    {
        return await _context.CartItems.FirstOrDefaultAsync(s => s.CartItemId == id);
    }
    
    public async Task<List<CartItem>> FindByCartId(int id)
    {
        var shoppingCart = await _context.ShoppingCarts.FirstOrDefaultAsync(s => s.CartId == id);
        
        if (shoppingCart == null) return null;
        
        return await _context.CartItems.Where(s => s.CartId == id).Include(x => x.Product).ThenInclude(x => x.ProdCat).ToListAsync();
    }
}
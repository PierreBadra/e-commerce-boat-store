using pbadraH60Customer.DTO;
using pbadraH60Customer.Models;

namespace pbadraH60Customer.DAL;

public interface ICartItemsRepository
{
    public Task<bool> Create(CartItem cartItem);
    
    public Task<bool> Update(CartItem cartItem);
    
    public Task<CartItem> Find(int id);
    
    public Task<bool> Delete(CartItem cartItem);
    
    public Task<List<CartItemDTO>> FindByShoppingCartId(int shoppingCartId);
}
namespace pbadraH60Customer.DAL;

public interface IShoppingCartRepository<T> where T : class
{
    public Task<bool> Create(T newShoppingCart);
    public Task<bool> Update(T updatedShoppingCart);
    public Task<T> Find(int shoppingCartId);
    public Task<T> FindByCustomerId(int customerId);
    public Task<bool> Delete(T deletedShoppingCart);
}
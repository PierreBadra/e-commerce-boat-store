namespace pbadraH60Store.DAL;

public interface IShoppingCartRepository<T> where T : class
{
    public Task<bool> Create(T newShoppingCart);
    public Task<bool> Update(T updatedShoppingCart);
    public Task<T> Find(int shoppingCartId);
    public Task<bool> Delete(T deletedShoppingCart);
}
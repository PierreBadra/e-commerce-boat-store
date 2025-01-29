using pbadraH60Services.Models;

namespace pbadraH60Services.DAL;

public interface IShoppingCartRepository<T> where T : class
{
    public Task<bool> Create(T newShoppingCart);
    public Task<List<T>> Read();
    public Task<bool> Update(T updatedShoppingCart);
    public Task<bool> Delete(int id);
    public Task<T> Find(int id);
    public Task<T> FindByCustomerId(int customerId);
}
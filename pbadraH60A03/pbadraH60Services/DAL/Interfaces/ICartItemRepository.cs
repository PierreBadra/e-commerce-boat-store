namespace pbadraH60Services.DAL;

public interface ICartItemRepository<T> where T : class
{
    public Task<bool> Create(T newCartItem);
    public Task<List<T>> Read();
    public Task<bool> Update(T updatedCartItem);
    public Task<bool> Delete(int id);
    public Task<T> Find(int id);
    public Task<List<T>> FindByCartId(int id);
}
namespace pbadraH60Services.DAL;

public interface IOrderItemRepository<T> where T : class
{
    public Task<bool> Create(T newOrderItem);
    public Task<List<T>> Read();
    public Task<bool> Update(T updatedOrderItem);
    public Task<T> Find(int id);
    public Task<List<T>> FindByOrderId(int id);
}
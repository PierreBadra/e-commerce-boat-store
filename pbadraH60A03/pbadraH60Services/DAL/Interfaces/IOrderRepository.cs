namespace pbadraH60Services.DAL;

public interface IOrderRepository<T> where T : class
{
    public Task<List<T>> ReadByDate(DateTime date);
    public Task<List<T>> ReadByCustomerName(string customerEmail);
    public Task<List<T>> Read();
    public Task<List<T>> ReadByCustomerId(int customerId);
    public Task<bool> Update(T orderDTO);
    public Task<bool> Create(T orderDTO);
    public Task<T> Find(int id);
}
using pbadraH60Customer.DTO;

namespace pbadraH60Customer.DAL;

public interface IOrderRepository
{
    public Task<bool> Create(OrderDTO newOrder);
    public Task<bool> Update(OrderDTO updatedOrder);
    public Task<OrderDTO> Find(int orderId);
    public Task<List<OrderDTO>> FindByCustomerName(string customerName);
    
    public Task<List<OrderDTO>> FindByCustomerId(int customerId);
}
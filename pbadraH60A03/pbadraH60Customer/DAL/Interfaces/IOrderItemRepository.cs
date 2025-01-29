using pbadraH60Customer.Models;

namespace pbadraH60Customer.DAL;

public interface IOrderItemRepository
{
    public Task<bool> Create(List<OrderItemDTO> orderItems);
    public Task<bool> Update(OrderItemDTO orderItem);
    public Task<OrderItemDTO> Find(int id);
    public Task<List<OrderItemDTO>> FindByOrderId(int orderId);
}
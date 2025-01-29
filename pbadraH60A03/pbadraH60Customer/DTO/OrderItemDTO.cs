namespace pbadraH60Customer.Models;

public class OrderItemDTO
{
    public int OrderItemId { get; set; }

    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }
    
    public OrderItemDTO() {}

    public OrderItemDTO(OrderItem orderItem)
    {
        OrderItemId = orderItem.OrderItemId;
        OrderId = orderItem.OrderId;
        ProductId = orderItem.ProductId;
        Quantity = orderItem.Quantity;
        Price = orderItem.Price;
    }
}
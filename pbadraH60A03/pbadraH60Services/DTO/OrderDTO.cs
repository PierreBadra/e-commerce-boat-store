using pbadraH60Services.Models;

namespace pbadraH60Services.DTO;

public class OrderDTO
{
    public int OrderId { get; set; }

    public int CustomerId { get; set; }

    public string CustomerName { get; set; } = null!;
    public string CustomerEmail { get; set; } = null!;
    public DateTime DateCreated { get; set; }

    public DateTime? DateFulfilled { get; set; }

    public decimal Total { get; set; }

    public decimal Taxes { get; set; }
    public OrderDTO() {}

    public OrderDTO(Order order)
    {
        CustomerEmail = order.Customer.Email;
        CustomerName = $"{order.Customer.FirstName} {order.Customer.LastName}".Trim();
        OrderId = order.OrderId;
        CustomerId = order.CustomerId;
        DateCreated = order.DateCreated;
        DateFulfilled = order.DateFulfilled;
        Total = order.Total;
        Taxes = order.Taxes;
    }
}
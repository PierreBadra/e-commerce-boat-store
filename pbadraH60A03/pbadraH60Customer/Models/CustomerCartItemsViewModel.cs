using pbadraH60Customer.DTO;

namespace pbadraH60Customer.Models;

public class CustomerCartItemsViewModel
{
    public Customer Customer { get; set; }
    public List<CartItemDTO> CartItems { get; set; }
}
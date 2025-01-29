using System.ComponentModel.DataAnnotations;
using pbadraH60Services.Models;

namespace pbadraH60Services.DTO;

public class CartItemDTO
{
    public int CartItemId { get; set; }

    public int CartId { get; set; }

    public int ProductId { get; set; }

    [Range(0, int.MaxValue)]
    public int Quantity { get; set; }

    public decimal Price { get; set; }
    
    public ProductCustomerDisplayDTO? Product { get; set; }

    public CartItemDTO(){}
    public CartItemDTO(CartItem cartItem)
    {
        CartItemId = cartItem.CartItemId;
        CartId = cartItem.CartId;
        ProductId = cartItem.ProductId;
        Quantity = cartItem.Quantity;
        Price = cartItem.Price;
        Product = new ProductCustomerDisplayDTO(cartItem.Product);
    }
}
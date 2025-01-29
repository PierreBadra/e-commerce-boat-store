using Newtonsoft.Json;
using pbadraH60Services.DTO;

namespace pbadraH60Services.Models;

public partial class CartItem
{
    public int CartItemId { get; set; }

    public int CartId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }
    
    public virtual ShoppingCart ShoppingCart { get; set; } = null!;
    
    public virtual Product Product { get; set; } = null!;
}

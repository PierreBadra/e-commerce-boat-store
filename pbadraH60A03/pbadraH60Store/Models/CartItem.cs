using Newtonsoft.Json;

namespace pbadraH60A01.Models
{
    public partial class CartItem
    {
        public int CartItemId { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        [JsonIgnore]
        public virtual ShoppingCart ShoppingCart { get; set; } = null!;
        
        [JsonIgnore]
        public virtual Product Product { get; set; } = null!;
    }
}

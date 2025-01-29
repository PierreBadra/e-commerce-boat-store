using pbadraH60Services.Models;

namespace pbadraH60Services.DTO
{
    public class ShoppingCartDTO
    {
        public int CartId { get; set; }
        public int CustomerId { get; set; }
        public DateTime DateCreated { get; set; }
        
        public ShoppingCartDTO(){}
        public ShoppingCartDTO(ShoppingCart shoppingCart)
        {
            CartId = shoppingCart.CartId;
            CustomerId = shoppingCart.CustomerId;
            DateCreated = shoppingCart.DateCreated;
        }
    }
}
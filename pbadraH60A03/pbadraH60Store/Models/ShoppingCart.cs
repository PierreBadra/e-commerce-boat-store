using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace pbadraH60A01.Models
{
    [PrimaryKey(nameof(CartId))]
    public partial class ShoppingCart
    {
        public int CartId { get; set; }
        public int CustomerId { get; set; }
        public DateTime DateCreated { get; set; }

        public virtual Customer Customer { get; set; } = null!;
        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}

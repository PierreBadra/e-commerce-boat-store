using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace pbadraH60Services.Models;

public partial class ShoppingCart
{
    public int CartId { get; set; }

    public int CustomerId { get; set; }

    public DateTime DateCreated { get; set; }

    [JsonIgnore]
    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    [JsonIgnore]
    public virtual Customer Customer { get; set; } = null!;
}

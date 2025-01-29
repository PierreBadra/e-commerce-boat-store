﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace pbadraH60Services.Models;

public partial class OrderItem
{
    public int OrderItemId { get; set; }

    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    [JsonIgnore]
    public virtual Order Order { get; set; } = null!;

    [JsonIgnore]
    public virtual Product Product { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace Library.Models;

public partial class OrderItem
{
    public int OrderItemId { get; set; }

    public int OrderId { get; set; }

    public int BookId { get; set; }

    public decimal Price { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}

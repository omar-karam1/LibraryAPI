using System;
using System.Collections.Generic;

namespace Library.Models;

public partial class Transaction
{
    public int TransactionId { get; set; }

    public int OrderId { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public DateTime? TransactionDate { get; set; }

    public decimal Amount { get; set; }

    public virtual Order Order { get; set; } = null!;
}

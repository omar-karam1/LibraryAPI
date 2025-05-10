using System;
using System.Collections.Generic;

namespace Library.Models;

public partial class Book
{
    public int BookId { get; set; }

    public string Title { get; set; } = null!;

    public int CategoryId { get; set; }

    public decimal? Price { get; set; }

    public bool? IsFree { get; set; }

    public string? FilePath { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? BookImage { get; set; }

    public string? BookDescription { get; set; }

    public virtual Category Category { get; set; } = null!;


    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<Author> Authors { get; set; } = new List<Author>();
}

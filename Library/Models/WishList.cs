using System;
using System.Collections.Generic;

namespace Library.Models;

public partial class WishList
{
    public int WishListId { get; set; }

    public int UserId { get; set; }

    public int BookId { get; set; }

    public DateTime? AddedDate { get; set; }

    public virtual User User { get; set; } = null!;
}

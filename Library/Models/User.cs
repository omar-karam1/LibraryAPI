using System;
using System.Collections.Generic;

namespace Library.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? UserType { get; set; }

    public string? ProfileImage { get; set; }

    public DateTime? BirthDate { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<WishList> WishLists { get; set; } = new List<WishList>();
}

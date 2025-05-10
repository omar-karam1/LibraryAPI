using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Library.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}

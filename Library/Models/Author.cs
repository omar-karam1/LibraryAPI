using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Library.Models;

public partial class Author
{
    public int AuthorId { get; set; }

    public string Name { get; set; } = null!;

    public string? AuthorImage { get; set; }

    public string? AboutAuthor { get; set; }
    [JsonIgnore]
    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}

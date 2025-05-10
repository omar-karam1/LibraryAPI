using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Library.Models;

public partial class Review
{
    public int ReviewId { get; set; }

    public int BookId { get; set; }

    public int UserId { get; set; }

    public byte? Rating { get; set; }

    public string? ReviewText { get; set; }

    public DateTime? CreatedDate { get; set; }
    [JsonIgnore]
    public virtual Book Book { get; set; } = null!;
    [JsonIgnore]
    public virtual User User { get; set; } = null!;
}

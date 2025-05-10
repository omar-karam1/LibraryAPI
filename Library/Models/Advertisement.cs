using System;
using System.Collections.Generic;

namespace Library.Models;

public partial class Advertisement
{
    public int AdvertisementId { get; set; }

    public string Title { get; set; } = null!;

    public string? Content { get; set; }

    public string? ImagePath { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }
}

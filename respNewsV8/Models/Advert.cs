using System;
using System.Collections.Generic;

namespace respNewsV8.Models;

public partial class Advert
{
    public int AdvertId { get; set; }

    public string? AdvertName { get; set; }

    public string? AdvertContext { get; set; }

    public string? AdvertCoverUrl { get; set; }

    public bool? AdvertVisibility { get; set; }

    public string? AdvertVideoUrl { get; set; }
}

using System;
using System.Collections.Generic;

namespace respNewsV8.Models;

public partial class AdditionalLink
{
    public int LinkId { get; set; }

    public string? LinkName { get; set; }

    public string? LinkUrl { get; set; }

    public string? LinkCoverUrl { get; set; }

    public bool? LinkVisibility { get; set; }
}

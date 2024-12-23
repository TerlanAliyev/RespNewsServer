using System;
using System.Collections.Generic;

namespace respNewsV8.Models;

public partial class Logo
{
    public int LogoId { get; set; }

    public string? LogoName { get; set; }

    public string? LogoCoverUrl { get; set; }

    public bool? LogoVisibility { get; set; }
}

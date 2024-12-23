using System;
using System.Collections.Generic;

namespace respNewsV8.Models;

public partial class AdvertDto
{
    public int AdvertId { get; set; }

    public string? AdvertName { get; set; }
    public string? AdvertContext { get; set; }


    public IFormFile? AdvertCoverFile { get; set; }
    public IFormFile? AdvertVideoUrl { get; set; }


    public bool? AdvertVisibility { get; set; }
}

using System;
using System.Collections.Generic;

namespace respNewsV8.Models;

public partial class LocationDatum
{
    public string Ipaddress { get; set; } = null!;

    public string? CountryName { get; set; }

    public string? CityName { get; set; }

    public string? Region { get; set; }

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }
}

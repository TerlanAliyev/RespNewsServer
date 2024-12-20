using Newtonsoft.Json;

public class GeoLocationModel
{
    [JsonProperty("country_name")]
    public string CountryName { get; set; }

    [JsonProperty("city")]
    public string City { get; set; }

    [JsonProperty("region_name")]
    public string RegionName { get; set; }

    [JsonProperty("continent_name")]
    public string ContinentName { get; set; }

    [JsonProperty("latitude")]
    public double Latitude { get; set; }

    [JsonProperty("longitude")]
    public double Longitude { get; set; }
}

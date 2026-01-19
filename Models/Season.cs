using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace f1_api.Models;

/// <summary>
/// Represents an F1 season
/// </summary>
public class Season
{
    /// <summary>
    /// Season year
    /// </summary>
    [Required]
    [JsonPropertyName("season")]
    public string Year { get; set; } = string.Empty;
    
    /// <summary>
    /// Wikipedia URL for the season
    /// </summary>
    [Required]
    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;
}

public class SeasonsResponse
{
    [JsonPropertyName("MRData")]
    public MRDataSeasons? MRData { get; set; }
}

public class MRDataSeasons
{
    [JsonPropertyName("xmlns")]
    public string Xmlns { get; set; } = string.Empty;
    
    [JsonPropertyName("series")]
    public string Series { get; set; } = string.Empty;
    
    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;
    
    [JsonPropertyName("limit")]
    public string Limit { get; set; } = string.Empty;
    
    [JsonPropertyName("offset")]
    public string Offset { get; set; } = string.Empty;
    
    [JsonPropertyName("total")]
    public string Total { get; set; } = string.Empty;
    
    [JsonPropertyName("SeasonTable")]
    public SeasonTable? SeasonTable { get; set; }
}

public class SeasonTable
{
    [JsonPropertyName("Seasons")]
    public List<Season> Seasons { get; set; } = new();
}

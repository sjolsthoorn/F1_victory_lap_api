using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace f1_api.Models;

/// <summary>
/// Represents an F1 race
/// </summary>
public class Race
{
    /// <summary>
    /// Season year
    /// </summary>
    [Required]
    [JsonPropertyName("season")]
    public string Season { get; set; } = string.Empty;
    
    /// <summary>
    /// Round number in the season
    /// </summary>
    [Required]
    [JsonPropertyName("round")]
    public string Round { get; set; } = string.Empty;
    
    /// <summary>
    /// Race name
    /// </summary>
    [Required]
    [JsonPropertyName("raceName")]
    public string RaceName { get; set; } = string.Empty;
    
    /// <summary>
    /// Circuit information
    /// </summary>
    [Required]
    [JsonPropertyName("Circuit")]
    public Circuit Circuit { get; set; } = new();
    
    /// <summary>
    /// Race date (ISO 8601 format)
    /// </summary>
    [Required]
    [JsonPropertyName("date")]
    public string Date { get; set; } = string.Empty;
    
    /// <summary>
    /// Race time (ISO 8601 format)
    /// </summary>
    [JsonPropertyName("time")]
    public string? Time { get; set; }
    
    /// <summary>
    /// Wikipedia URL for the race
    /// </summary>
    [Required]
    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;
    
    /// <summary>
    /// Race results (when included in response)
    /// </summary>
    [JsonPropertyName("Results")]
    public List<Result>? Results { get; set; }
}

/// <summary>
/// Represents an F1 circuit
/// </summary>
public class Circuit
{
    /// <summary>
    /// Circuit identifier
    /// </summary>
    [Required]
    [JsonPropertyName("circuitId")]
    public string CircuitId { get; set; } = string.Empty;
    
    /// <summary>
    /// Circuit name
    /// </summary>
    [Required]
    [JsonPropertyName("circuitName")]
    public string CircuitName { get; set; } = string.Empty;
    
    /// <summary>
    /// Location information
    /// </summary>
    [Required]
    [JsonPropertyName("Location")]
    public Location Location { get; set; } = new();
    
    /// <summary>
    /// Wikipedia URL for the circuit
    /// </summary>
    [Required]
    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;
}

/// <summary>
/// Represents a circuit location
/// </summary>
public class Location
{
    /// <summary>
    /// Latitude
    /// </summary>
    [Required]
    [JsonPropertyName("lat")]
    public string Latitude { get; set; } = string.Empty;
    
    /// <summary>
    /// Longitude
    /// </summary>
    [Required]
    [JsonPropertyName("long")]
    public string Longitude { get; set; } = string.Empty;
    
    /// <summary>
    /// Locality (city)
    /// </summary>
    [Required]
    [JsonPropertyName("locality")]
    public string Locality { get; set; } = string.Empty;
    
    /// <summary>
    /// Country
    /// </summary>
    [Required]
    [JsonPropertyName("country")]
    public string Country { get; set; } = string.Empty;
}

public class RacesResponse
{
    [JsonPropertyName("MRData")]
    public MRDataRaces? MRData { get; set; }
}

public class MRDataRaces
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
    
    [JsonPropertyName("RaceTable")]
    public RaceTable? RaceTable { get; set; }
}

public class RaceTable
{
    [JsonPropertyName("season")]
    public string? Season { get; set; }
    
    [JsonPropertyName("round")]
    public string? Round { get; set; }
    
    [JsonPropertyName("Races")]
    public List<Race> Races { get; set; } = new();
}

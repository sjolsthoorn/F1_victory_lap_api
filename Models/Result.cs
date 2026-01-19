using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace f1_api.Models;

/// <summary>
/// Represents a race result
/// </summary>
public class Result
{
    /// <summary>
    /// Race number
    /// </summary>
    [Required]
    [JsonPropertyName("number")]
    public string Number { get; set; } = string.Empty;
    
    /// <summary>
    /// Final position
    /// </summary>
    [Required]
    [JsonPropertyName("position")]
    public string Position { get; set; } = string.Empty;
    
    /// <summary>
    /// Position text (can include "R" for retired, etc.)
    /// </summary>
    [Required]
    [JsonPropertyName("positionText")]
    public string PositionText { get; set; } = string.Empty;
    
    /// <summary>
    /// Points awarded
    /// </summary>
    [Required]
    [JsonPropertyName("points")]
    public string Points { get; set; } = string.Empty;
    
    /// <summary>
    /// Driver information
    /// </summary>
    [Required]
    [JsonPropertyName("Driver")]
    public Driver Driver { get; set; } = new();
    
    /// <summary>
    /// Constructor (team) information
    /// </summary>
    [Required]
    [JsonPropertyName("Constructor")]
    public Constructor Constructor { get; set; } = new();
    
    /// <summary>
    /// Starting grid position
    /// </summary>
    [Required]
    [JsonPropertyName("grid")]
    public string Grid { get; set; } = string.Empty;
    
    /// <summary>
    /// Number of laps completed
    /// </summary>
    [Required]
    [JsonPropertyName("laps")]
    public string Laps { get; set; } = string.Empty;
    
    /// <summary>
    /// Finishing status (e.g., "Finished", "Retired")
    /// </summary>
    [Required]
    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;
    
    /// <summary>
    /// Fastest lap time information
    /// </summary>
    [JsonPropertyName("FastestLap")]
    public FastestLap? FastestLap { get; set; }
}

/// <summary>
/// Represents an F1 constructor (team)
/// </summary>
public class Constructor
{
    /// <summary>
    /// Constructor identifier
    /// </summary>
    [Required]
    [JsonPropertyName("constructorId")]
    public string ConstructorId { get; set; } = string.Empty;
    
    /// <summary>
    /// Constructor name
    /// </summary>
    [Required]
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Constructor nationality
    /// </summary>
    [Required]
    [JsonPropertyName("nationality")]
    public string Nationality { get; set; } = string.Empty;
    
    /// <summary>
    /// Wikipedia URL for the constructor
    /// </summary>
    [Required]
    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;
}

/// <summary>
/// Represents fastest lap information
/// </summary>
public class FastestLap
{
    /// <summary>
    /// Lap number
    /// </summary>
    [Required]
    [JsonPropertyName("rank")]
    public string Rank { get; set; } = string.Empty;
    
    /// <summary>
    /// Lap number
    /// </summary>
    [Required]
    [JsonPropertyName("lap")]
    public string Lap { get; set; } = string.Empty;
    
    /// <summary>
    /// Time information
    /// </summary>
    [JsonPropertyName("Time")]
    public TimeInfo? Time { get; set; }
    
    /// <summary>
    /// Average speed
    /// </summary>
    [JsonPropertyName("AverageSpeed")]
    public AverageSpeed? AverageSpeed { get; set; }
}

/// <summary>
/// Represents time information
/// </summary>
public class TimeInfo
{
    /// <summary>
    /// Time in milliseconds
    /// </summary>
    [JsonPropertyName("millis")]
    public string? Millis { get; set; }
    
    /// <summary>
    /// Time as string (e.g., "1:23.456")
    /// </summary>
    [Required]
    [JsonPropertyName("time")]
    public string Time { get; set; } = string.Empty;
}

/// <summary>
/// Represents average speed information
/// </summary>
public class AverageSpeed
{
    /// <summary>
    /// Speed units (e.g., "kph")
    /// </summary>
    [Required]
    [JsonPropertyName("units")]
    public string Units { get; set; } = string.Empty;
    
    /// <summary>
    /// Speed value
    /// </summary>
    [Required]
    [JsonPropertyName("speed")]
    public string Speed { get; set; } = string.Empty;
}

public class ResultsResponse
{
    [JsonPropertyName("MRData")]
    public MRDataResults? MRData { get; set; }
}

public class MRDataResults
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

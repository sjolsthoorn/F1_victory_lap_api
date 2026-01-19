using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace f1_api.Models;

/// <summary>
/// Represents driver championship standings
/// </summary>
public class DriverStanding
{
    /// <summary>
    /// Position in championship
    /// </summary>
    [Required]
    [JsonPropertyName("position")]
    public string Position { get; set; } = string.Empty;
    
    /// <summary>
    /// Position text
    /// </summary>
    [Required]
    [JsonPropertyName("positionText")]
    public string PositionText { get; set; } = string.Empty;
    
    /// <summary>
    /// Points total
    /// </summary>
    [Required]
    [JsonPropertyName("points")]
    public string Points { get; set; } = string.Empty;
    
    /// <summary>
    /// Number of wins
    /// </summary>
    [Required]
    [JsonPropertyName("wins")]
    public string Wins { get; set; } = string.Empty;
    
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
    [JsonPropertyName("Constructors")]
    public List<Constructor> Constructors { get; set; } = new();
}

/// <summary>
/// Represents constructor championship standings
/// </summary>
public class ConstructorStanding
{
    /// <summary>
    /// Position in championship
    /// </summary>
    [Required]
    [JsonPropertyName("position")]
    public string Position { get; set; } = string.Empty;
    
    /// <summary>
    /// Position text
    /// </summary>
    [Required]
    [JsonPropertyName("positionText")]
    public string PositionText { get; set; } = string.Empty;
    
    /// <summary>
    /// Points total
    /// </summary>
    [Required]
    [JsonPropertyName("points")]
    public string Points { get; set; } = string.Empty;
    
    /// <summary>
    /// Number of wins
    /// </summary>
    [Required]
    [JsonPropertyName("wins")]
    public string Wins { get; set; } = string.Empty;
    
    /// <summary>
    /// Constructor (team) information
    /// </summary>
    [Required]
    [JsonPropertyName("Constructor")]
    public Constructor Constructor { get; set; } = new();
}

public class DriverStandingsResponse
{
    [JsonPropertyName("MRData")]
    public MRDataDriverStandings? MRData { get; set; }
}

public class MRDataDriverStandings
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
    
    [JsonPropertyName("StandingsTable")]
    public DriverStandingsTable? StandingsTable { get; set; }
}

public class DriverStandingsTable
{
    [JsonPropertyName("season")]
    public string? Season { get; set; }
    
    [JsonPropertyName("round")]
    public string? Round { get; set; }
    
    [JsonPropertyName("StandingsLists")]
    public List<DriverStandingsList> StandingsLists { get; set; } = new();
}

public class DriverStandingsList
{
    [JsonPropertyName("season")]
    public string? Season { get; set; }
    
    [JsonPropertyName("round")]
    public string? Round { get; set; }
    
    [JsonPropertyName("DriverStandings")]
    public List<DriverStanding> DriverStandings { get; set; } = new();
}

public class ConstructorStandingsResponse
{
    [JsonPropertyName("MRData")]
    public MRDataConstructorStandings? MRData { get; set; }
}

public class MRDataConstructorStandings
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
    
    [JsonPropertyName("StandingsTable")]
    public ConstructorStandingsTable? StandingsTable { get; set; }
}

public class ConstructorStandingsTable
{
    [JsonPropertyName("season")]
    public string? Season { get; set; }
    
    [JsonPropertyName("round")]
    public string? Round { get; set; }
    
    [JsonPropertyName("StandingsLists")]
    public List<ConstructorStandingsList> StandingsLists { get; set; } = new();
}

public class ConstructorStandingsList
{
    [JsonPropertyName("season")]
    public string? Season { get; set; }
    
    [JsonPropertyName("round")]
    public string? Round { get; set; }
    
    [JsonPropertyName("ConstructorStandings")]
    public List<ConstructorStanding> ConstructorStandings { get; set; } = new();
}

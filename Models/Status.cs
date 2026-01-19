using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace f1_api.Models;

/// <summary>
/// Represents a finishing status code
/// </summary>
public class Status
{
    /// <summary>
    /// Status identifier
    /// </summary>
    [Required]
    [JsonPropertyName("statusId")]
    public string StatusId { get; set; } = string.Empty;
    
    /// <summary>
    /// Number of occurrences
    /// </summary>
    [Required]
    [JsonPropertyName("count")]
    public string Count { get; set; } = string.Empty;
    
    /// <summary>
    /// Status description (e.g., "Finished", "Retired")
    /// </summary>
    [Required]
    [JsonPropertyName("status")]
    public string StatusText { get; set; } = string.Empty;
}

public class StatusResponse
{
    [JsonPropertyName("MRData")]
    public MRDataStatus? MRData { get; set; }
}

public class MRDataStatus
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
    
    [JsonPropertyName("StatusTable")]
    public StatusTable? StatusTable { get; set; }
}

public class StatusTable
{
    [JsonPropertyName("Status")]
    public List<Status> Status { get; set; } = new();
}

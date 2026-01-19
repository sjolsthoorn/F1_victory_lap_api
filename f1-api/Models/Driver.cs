using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace f1_api.Models;

/// <summary>
/// Represents an F1 driver
/// </summary>
public class Driver
{
    /// <summary>
    /// Unique driver identifier
    /// </summary>
    [Required]
    [JsonPropertyName("driverId")]
    public string DriverId { get; set; } = string.Empty;
    
    /// <summary>
    /// Permanent driver number
    /// </summary>
    [JsonPropertyName("permanentNumber")]
    public string? PermanentNumber { get; set; }
    
    /// <summary>
    /// Driver code (3-letter abbreviation)
    /// </summary>
    [JsonPropertyName("code")]
    public string? Code { get; set; }
    
    /// <summary>
    /// Wikipedia URL for the driver
    /// </summary>
    [Required]
    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;
    
    /// <summary>
    /// Driver's first name
    /// </summary>
    [Required]
    [JsonPropertyName("givenName")]
    public string GivenName { get; set; } = string.Empty;
    
    /// <summary>
    /// Driver's last name
    /// </summary>
    [Required]
    [JsonPropertyName("familyName")]
    public string FamilyName { get; set; } = string.Empty;
    
    /// <summary>
    /// Driver's date of birth (ISO 8601 format)
    /// </summary>
    [Required]
    [JsonPropertyName("dateOfBirth")]
    public string DateOfBirth { get; set; } = string.Empty;
    
    /// <summary>
    /// Driver's nationality
    /// </summary>
    [Required]
    [JsonPropertyName("nationality")]
    public string Nationality { get; set; } = string.Empty;
    
    /// <summary>
    /// URL to driver's headshot image (constructed from Formula1.com)
    /// </summary>
    [JsonIgnore]
    public string? ImageUrl => GetDriverImageUrl();
    
    private string? GetDriverImageUrl()
    {
        // Formula1.com uses a pattern like: /content/dam/fom-website/drivers/{FIRSTLETTER}/{DRIVERCODE}_{GivenName}_{FamilyName}/{drivercode}.png
        // Example: MAXVER01_Max_Verstappen/maxver01.png
        
        if (string.IsNullOrEmpty(Code) || string.IsNullOrEmpty(GivenName) || string.IsNullOrEmpty(FamilyName))
        {
            return null;
        }
        
        var driverCodeUpper = Code.ToUpperInvariant();
        var firstLetter = driverCodeUpper[0];
        var driverCodeFormatted = $"{driverCodeUpper}{FamilyName.ToUpperInvariant().Substring(0, Math.Min(3, FamilyName.Length))}01";
        var driverNameFormatted = $"{driverCodeFormatted}_{GivenName}_{FamilyName}";
        var imageFileName = $"{Code.ToLowerInvariant()}.png";
        
        return $"https://www.formula1.com/content/dam/fom-website/drivers/{firstLetter}/{driverNameFormatted}/{imageFileName}";
    }
}

public class DriversResponse
{
    [JsonPropertyName("MRData")]
    public MRData? MRData { get; set; }
}

public class MRData
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
    
    [JsonPropertyName("DriverTable")]
    public DriverTable? DriverTable { get; set; }
}

public class DriverTable
{
    [JsonPropertyName("Drivers")]
    public List<Driver> Drivers { get; set; } = new();
}

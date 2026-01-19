using Microsoft.AspNetCore.Mvc;
using f1_api.Services;

namespace f1_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DriversController : ControllerBase
{
    private readonly IF1ApiService _f1ApiService;
    private readonly ILogger<DriversController> _logger;

    public DriversController(IF1ApiService f1ApiService, ILogger<DriversController> logger)
    {
        _f1ApiService = f1ApiService;
        _logger = logger;
    }

    /// <summary>
    /// Get all drivers from F1 history
    /// </summary>
    /// <param name="limit">Maximum number of results to return (default: 1000)</param>
    /// <param name="offset">Offset for pagination (default: 0)</param>
    /// <returns>List of drivers</returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<Models.Driver>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<Models.Driver>>> GetAllDrivers([FromQuery] int limit = 1000, [FromQuery] int offset = 0)
    {
        try
        {
            var drivers = await _f1ApiService.GetAllDriversAsync(limit, offset);
            return Ok(drivers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all drivers");
            return StatusCode(500, "An error occurred while fetching drivers");
        }
    }

    /// <summary>
    /// Get drivers for a specific season
    /// </summary>
    /// <param name="year">The F1 season year</param>
    /// <returns>List of drivers for the specified year</returns>
    [HttpGet("{year}")]
    [ProducesResponseType(typeof(List<Models.Driver>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<Models.Driver>>> GetDriversByYear(int year)
    {
        try
        {
            var drivers = await _f1ApiService.GetDriversByYearAsync(year);
            return Ok(drivers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting drivers for year {Year}", year);
            return StatusCode(500, $"An error occurred while fetching drivers for year {year}");
        }
    }

    /// <summary>
    /// Get a specific driver by ID
    /// </summary>
    /// <param name="driverId">The driver ID (e.g., 'alonso', 'hamilton')</param>
    /// <returns>Driver information</returns>
    [HttpGet("driver/{driverId}")]
    [ProducesResponseType(typeof(Models.Driver), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Models.Driver>> GetDriverById(string driverId)
    {
        try
        {
            var driver = await _f1ApiService.GetDriverByIdAsync(driverId);
            
            if (driver == null)
            {
                return NotFound($"Driver with ID '{driverId}' not found");
            }

            return Ok(driver);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting driver {DriverId}", driverId);
            return StatusCode(500, $"An error occurred while fetching driver {driverId}");
        }
    }
}

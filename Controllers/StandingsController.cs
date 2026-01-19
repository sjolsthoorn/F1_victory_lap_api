using Microsoft.AspNetCore.Mvc;
using f1_api.Services;

namespace f1_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StandingsController : ControllerBase
{
    private readonly IF1ApiService _f1ApiService;
    private readonly ILogger<StandingsController> _logger;

    public StandingsController(IF1ApiService f1ApiService, ILogger<StandingsController> logger)
    {
        _f1ApiService = f1ApiService;
        _logger = logger;
    }

    /// <summary>
    /// Get driver championship standings for a specific season
    /// </summary>
    /// <param name="year">The F1 season year</param>
    /// <param name="round">Optional round number to get standings at a specific point in the season</param>
    /// <returns>List of driver standings</returns>
    [HttpGet("drivers/{year}")]
    [ProducesResponseType(typeof(List<Models.DriverStanding>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<Models.DriverStanding>>> GetDriverStandings(int year, [FromQuery] int? round = null)
    {
        try
        {
            var standings = await _f1ApiService.GetDriverStandingsByYearAsync(year, round);
            return Ok(standings);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting driver standings for year {Year}, round {Round}", year, round);
            return StatusCode(500, $"An error occurred while fetching driver standings for year {year}");
        }
    }

    /// <summary>
    /// Get constructor championship standings for a specific season
    /// </summary>
    /// <param name="year">The F1 season year</param>
    /// <param name="round">Optional round number to get standings at a specific point in the season</param>
    /// <returns>List of constructor standings</returns>
    [HttpGet("constructors/{year}")]
    [ProducesResponseType(typeof(List<Models.ConstructorStanding>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<Models.ConstructorStanding>>> GetConstructorStandings(int year, [FromQuery] int? round = null)
    {
        try
        {
            var standings = await _f1ApiService.GetConstructorStandingsByYearAsync(year, round);
            return Ok(standings);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting constructor standings for year {Year}, round {Round}", year, round);
            return StatusCode(500, $"An error occurred while fetching constructor standings for year {year}");
        }
    }
}

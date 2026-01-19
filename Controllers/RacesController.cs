using Microsoft.AspNetCore.Mvc;
using f1_api.Services;

namespace f1_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RacesController : ControllerBase
{
    private readonly IF1ApiService _f1ApiService;
    private readonly ILogger<RacesController> _logger;

    public RacesController(IF1ApiService f1ApiService, ILogger<RacesController> logger)
    {
        _f1ApiService = f1ApiService;
        _logger = logger;
    }

    /// <summary>
    /// Get all races for a specific season
    /// </summary>
    /// <param name="year">The F1 season year</param>
    /// <param name="limit">Maximum number of results to return (default: 1000)</param>
    /// <param name="offset">Offset for pagination (default: 0)</param>
    /// <returns>List of races for the specified year</returns>
    [HttpGet("{year}")]
    [ProducesResponseType(typeof(List<Models.Race>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<Models.Race>>> GetRacesByYear(int year, [FromQuery] int limit = 1000, [FromQuery] int offset = 0)
    {
        try
        {
            var races = await _f1ApiService.GetRacesByYearAsync(year, limit, offset);
            return Ok(races);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting races for year {Year}", year);
            return StatusCode(500, $"An error occurred while fetching races for year {year}");
        }
    }

    /// <summary>
    /// Get a specific race by year and round
    /// </summary>
    /// <param name="year">The F1 season year</param>
    /// <param name="round">The race round number</param>
    /// <returns>Race information</returns>
    [HttpGet("{year}/{round}")]
    [ProducesResponseType(typeof(Models.Race), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Models.Race>> GetRaceByYearAndRound(int year, int round)
    {
        try
        {
            var race = await _f1ApiService.GetRaceByYearAndRoundAsync(year, round);
            
            if (race == null)
            {
                return NotFound($"Race for year {year}, round {round} not found");
            }

            return Ok(race);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting race {Year}/{Round}", year, round);
            return StatusCode(500, $"An error occurred while fetching race {year}/{round}");
        }
    }
}

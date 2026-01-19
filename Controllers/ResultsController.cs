using Microsoft.AspNetCore.Mvc;
using f1_api.Services;

namespace f1_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ResultsController : ControllerBase
{
    private readonly IF1ApiService _f1ApiService;
    private readonly ILogger<ResultsController> _logger;

    public ResultsController(IF1ApiService f1ApiService, ILogger<ResultsController> logger)
    {
        _f1ApiService = f1ApiService;
        _logger = logger;
    }

    /// <summary>
    /// Get all race results for a specific season
    /// </summary>
    /// <param name="year">The F1 season year</param>
    /// <param name="limit">Maximum number of results to return (default: 1000)</param>
    /// <param name="offset">Offset for pagination (default: 0)</param>
    /// <returns>List of race results for the specified year</returns>
    [HttpGet("{year}")]
    [ProducesResponseType(typeof(List<Models.Result>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<Models.Result>>> GetResultsByYear(int year, [FromQuery] int limit = 1000, [FromQuery] int offset = 0)
    {
        try
        {
            var results = await _f1ApiService.GetResultsByYearAsync(year, limit, offset);
            return Ok(results);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting results for year {Year}", year);
            return StatusCode(500, $"An error occurred while fetching results for year {year}");
        }
    }

    /// <summary>
    /// Get race results for a specific race
    /// </summary>
    /// <param name="year">The F1 season year</param>
    /// <param name="round">The race round number</param>
    /// <returns>List of race results for the specified race</returns>
    [HttpGet("{year}/{round}")]
    [ProducesResponseType(typeof(List<Models.Result>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<Models.Result>>> GetResultsByYearAndRound(int year, int round)
    {
        try
        {
            var results = await _f1ApiService.GetResultsByYearAndRoundAsync(year, round);
            return Ok(results);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting results for {Year}/{Round}", year, round);
            return StatusCode(500, $"An error occurred while fetching results for {year}/{round}");
        }
    }
}

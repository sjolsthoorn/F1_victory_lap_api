using Microsoft.AspNetCore.Mvc;
using f1_api.Services;

namespace f1_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SeasonsController : ControllerBase
{
    private readonly IF1ApiService _f1ApiService;
    private readonly ILogger<SeasonsController> _logger;

    public SeasonsController(IF1ApiService f1ApiService, ILogger<SeasonsController> logger)
    {
        _f1ApiService = f1ApiService;
        _logger = logger;
    }

    /// <summary>
    /// Get all F1 seasons
    /// </summary>
    /// <param name="limit">Maximum number of results to return (default: 1000)</param>
    /// <param name="offset">Offset for pagination (default: 0)</param>
    /// <returns>List of seasons</returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<Models.Season>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<Models.Season>>> GetAllSeasons([FromQuery] int limit = 1000, [FromQuery] int offset = 0)
    {
        try
        {
            var seasons = await _f1ApiService.GetAllSeasonsAsync(limit, offset);
            return Ok(seasons);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all seasons");
            return StatusCode(500, "An error occurred while fetching seasons");
        }
    }
}

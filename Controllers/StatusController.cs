using Microsoft.AspNetCore.Mvc;
using f1_api.Services;

namespace f1_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatusController : ControllerBase
{
    private readonly IF1ApiService _f1ApiService;
    private readonly ILogger<StatusController> _logger;

    public StatusController(IF1ApiService f1ApiService, ILogger<StatusController> logger)
    {
        _f1ApiService = f1ApiService;
        _logger = logger;
    }

    /// <summary>
    /// Get all finishing status codes
    /// </summary>
    /// <param name="limit">Maximum number of results to return (default: 1000)</param>
    /// <param name="offset">Offset for pagination (default: 0)</param>
    /// <returns>List of status codes</returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<Models.Status>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<Models.Status>>> GetAllStatus([FromQuery] int limit = 1000, [FromQuery] int offset = 0)
    {
        try
        {
            var statuses = await _f1ApiService.GetAllStatusAsync(limit, offset);
            return Ok(statuses);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all status");
            return StatusCode(500, "An error occurred while fetching status");
        }
    }
}

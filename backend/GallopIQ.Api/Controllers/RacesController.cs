using GallopIQ.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace GallopIQ.Api.Controllers;

[ApiController]
[Route("races")]
public class RacesController : ControllerBase
{
    private readonly IRacingApiService _racingApiService;
    private readonly ILogger<RacesController> _logger;

    public RacesController(IRacingApiService racingApiService, ILogger<RacesController> logger)
    {
        _racingApiService = racingApiService;
        _logger = logger;
    }

    [HttpGet("today")]
    public async Task<IActionResult> GetTodaysRaces()
    {
        try
        {
            var races = await _racingApiService.GetTodaysRacesAsync();
            return Ok(races);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching today's races");
            return StatusCode(500, new { error = "Failed to fetch races" });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRace(string id)
    {
        try
        {
            var race = await _racingApiService.GetRaceByIdAsync(id);
            if (race == null) return NotFound(new { error = $"Race {id} not found" });
            return Ok(race);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching race {RaceId}", id);
            return StatusCode(500, new { error = "Failed to fetch race" });
        }
    }
}

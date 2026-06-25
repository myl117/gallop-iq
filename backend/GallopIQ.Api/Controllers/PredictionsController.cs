using GallopIQ.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace GallopIQ.Api.Controllers;

[ApiController]
[Route("")]
public class PredictionsController : ControllerBase
{
    private readonly IPredictionService _predictionService;
    private readonly ILogger<PredictionsController> _logger;

    public PredictionsController(IPredictionService predictionService, ILogger<PredictionsController> logger)
    {
        _predictionService = predictionService;
        _logger = logger;
    }

    [HttpPost("predict/{raceId}")]
    public async Task<IActionResult> Predict(string raceId)
    {
        try
        {
            var result = await _predictionService.GeneratePredictionsAsync(raceId);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Race not found for prediction: {RaceId}", raceId);
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating predictions for race {RaceId}", raceId);
            return StatusCode(500, new { error = "Failed to generate predictions", detail = ex.Message });
        }
    }

    [HttpGet("predictions/{raceId}")]
    public IActionResult GetPredictions(string raceId)
    {
        var result = _predictionService.GetStoredPredictions(raceId);
        if (result == null) return NotFound(new { error = $"No predictions found for race {raceId}" });
        return Ok(result);
    }
}

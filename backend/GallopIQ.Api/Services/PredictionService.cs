using GallopIQ.Api.DTOs;
using GallopIQ.Api.Stores;

namespace GallopIQ.Api.Services;

public class PredictionService : IPredictionService
{
    private readonly IRacingApiService _racingApiService;
    private readonly IFeatureBuilderService _featureBuilderService;
    private readonly IGeminiService _geminiService;
    private readonly PredictionStore _store;
    private readonly ILogger<PredictionService> _logger;

    public PredictionService(
        IRacingApiService racingApiService,
        IFeatureBuilderService featureBuilderService,
        IGeminiService geminiService,
        PredictionStore store,
        ILogger<PredictionService> logger)
    {
        _racingApiService = racingApiService;
        _featureBuilderService = featureBuilderService;
        _geminiService = geminiService;
        _store = store;
        _logger = logger;
    }

    public async Task<RacePredictionResultDto> GeneratePredictionsAsync(string raceId)
    {
        // 1. Fetch race detail
        var race = await _racingApiService.GetRaceByIdAsync(raceId);
        if (race == null)
            throw new InvalidOperationException($"Race '{raceId}' not found.");

        _logger.LogInformation("Generating predictions for race {RaceId} ({RaceName})", raceId, race.RaceName);

        // 2. Build feature vectors
        var features = _featureBuilderService.BuildFeatures(race);

        // 3. Call Gemini
        var geminiItems = await _geminiService.GetPredictionsAsync(race, features);

        // 4. Map to PredictionDto, sorted descending by WinProbability
        var predictions = geminiItems
            .Select(item => new PredictionDto
            {
                HorseName = item.HorseName,
                WinProbability = item.WinProbability,
                Confidence = item.Confidence,
                ValueScore = item.ValueScore,
                Reasoning = item.Reasoning,
                IsBestPick = false
            })
            .OrderByDescending(p => p.WinProbability)
            .ToList();

        // 5. Mark the best pick
        if (predictions.Count > 0)
            predictions[0].IsBestPick = true;

        // 6. Build result and persist
        var result = new RacePredictionResultDto
        {
            RaceId = raceId,
            GeneratedAt = DateTime.UtcNow,
            Horses = predictions
        };

        _store.Save(raceId, result);
        _logger.LogInformation("Saved {Count} predictions for race {RaceId}", predictions.Count, raceId);

        return result;
    }

    public RacePredictionResultDto? GetStoredPredictions(string raceId) =>
        _store.Get(raceId);
}

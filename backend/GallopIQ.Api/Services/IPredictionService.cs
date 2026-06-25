using GallopIQ.Api.DTOs;

namespace GallopIQ.Api.Services;

public interface IPredictionService
{
    Task<RacePredictionResultDto> GeneratePredictionsAsync(string raceId);
    RacePredictionResultDto? GetStoredPredictions(string raceId);
}

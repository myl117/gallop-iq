using GallopIQ.Api.DTOs;

namespace GallopIQ.Api.Services;

public interface IGeminiService
{
    Task<List<GeminiPredictionItem>> GetPredictionsAsync(RaceDetailDto race, List<HorseFeatureDto> features);
}

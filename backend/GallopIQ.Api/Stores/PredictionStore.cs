using System.Collections.Concurrent;
using GallopIQ.Api.DTOs;

namespace GallopIQ.Api.Stores;

public class PredictionStore
{
    private readonly ConcurrentDictionary<string, RacePredictionResultDto> _cache = new();

    public void Save(string raceId, RacePredictionResultDto result) =>
        _cache[raceId] = result;

    public RacePredictionResultDto? Get(string raceId) =>
        _cache.TryGetValue(raceId, out var result) ? result : null;

    public bool HasPrediction(string raceId) =>
        _cache.ContainsKey(raceId);
}

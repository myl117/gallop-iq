using System.Net.Http.Json;
using System.Text.Json;
using GallopIQ.Api.DTOs;

namespace GallopIQ.Api.Services;

public class RacingApiService : IRacingApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<RacingApiService> _logger;

    public RacingApiService(IHttpClientFactory factory, ILogger<RacingApiService> logger)
    {
        _httpClient = factory.CreateClient("RacingApi");
        _logger = logger;
    }

    public async Task<List<RaceDto>> GetTodaysRacesAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("/v1/racecards/free");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var racecardResponse = JsonSerializer.Deserialize<RacingApiRacecardResponse>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return racecardResponse?.Racecards
                .Select(MapToRaceDto)
                .ToList() ?? new List<RaceDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching today's racecards from Racing API");
            return new List<RaceDto>();
        }
    }

    public async Task<RaceDetailDto?> GetRaceByIdAsync(string raceId)
    {
        try
        {
            var response = await _httpClient.GetAsync("/v1/racecards/free");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var racecardResponse = JsonSerializer.Deserialize<RacingApiRacecardResponse>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var racecard = racecardResponse?.Racecards
                .FirstOrDefault(r => r.RaceId == raceId);

            return racecard == null ? null : MapToRaceDetailDto(racecard);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching race {RaceId} from Racing API", raceId);
            return null;
        }
    }

    private static RaceDto MapToRaceDto(RacingApiRacecard racecard) => new()
    {
        RaceId = racecard.RaceId,
        CourseName = racecard.Course,
        RaceName = racecard.RaceName,
        OffTime = racecard.OffTime,
        Distance = racecard.DistanceF,
        Going = racecard.Going,
        RaceClass = racecard.RaceClass,
        RegionCode = racecard.Region,
        RunnerCount = racecard.Runners.Count,
        Date = racecard.Date
    };

    private static RaceDetailDto MapToRaceDetailDto(RacingApiRacecard racecard) => new()
    {
        RaceId = racecard.RaceId,
        CourseName = racecard.Course,
        RaceName = racecard.RaceName,
        OffTime = racecard.OffTime,
        Distance = racecard.DistanceF,
        Going = racecard.Going,
        RaceClass = racecard.RaceClass,
        RegionCode = racecard.Region,
        RunnerCount = racecard.Runners.Count,
        Date = racecard.Date,
        Runners = racecard.Runners.Select(r => new RacecardRunnerDto
        {
            HorseName = r.Horse,
            Jockey = r.Jockey,
            Trainer = r.Trainer,
            Age = r.Age,
            Form = r.Form,
            Number = int.TryParse(r.Number, out var num) ? num : null,
            Odds = r.Sp,
            Lbs = r.Lbs
        }).ToList()
    };
}

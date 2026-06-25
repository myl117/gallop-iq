using GallopIQ.Api.DTOs;

namespace GallopIQ.Api.Services;

public interface IRacingApiService
{
    Task<List<RaceDto>> GetTodaysRacesAsync();
    Task<RaceDetailDto?> GetRaceByIdAsync(string raceId);
}

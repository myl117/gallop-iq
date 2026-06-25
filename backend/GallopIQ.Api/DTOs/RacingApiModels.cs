using System.Text.Json.Serialization;

namespace GallopIQ.Api.DTOs;

public class RacingApiRacecardResponse
{
    [JsonPropertyName("racecards")]
    public List<RacingApiRacecard> Racecards { get; set; } = new();
}

public class RacingApiRacecard
{
    [JsonPropertyName("race_id")]
    public string RaceId { get; set; } = string.Empty;

    [JsonPropertyName("course")]
    public string Course { get; set; } = string.Empty;

    [JsonPropertyName("course_id")]
    public string CourseId { get; set; } = string.Empty;

    [JsonPropertyName("date")]
    public string Date { get; set; } = string.Empty;

    [JsonPropertyName("off_time")]
    public string OffTime { get; set; } = string.Empty;

    [JsonPropertyName("race_name")]
    public string RaceName { get; set; } = string.Empty;

    [JsonPropertyName("distance_f")]
    public string? DistanceF { get; set; }

    [JsonPropertyName("going")]
    public string? Going { get; set; }

    [JsonPropertyName("race_class")]
    public string? RaceClass { get; set; }

    [JsonPropertyName("region")]
    public string? Region { get; set; }

    [JsonPropertyName("runners")]
    public List<RacingApiRunner> Runners { get; set; } = new();
}

public class RacingApiRunner
{
    [JsonPropertyName("horse")]
    public string Horse { get; set; } = string.Empty;

    [JsonPropertyName("jockey")]
    public string? Jockey { get; set; }

    [JsonPropertyName("trainer")]
    public string? Trainer { get; set; }

    [JsonPropertyName("age")]
    public string? Age { get; set; }

    [JsonPropertyName("lbs")]
    public string? Lbs { get; set; }

    [JsonPropertyName("form")]
    public string? Form { get; set; }

    [JsonPropertyName("number")]
    public string? Number { get; set; }

    [JsonPropertyName("sp")]
    public string? Sp { get; set; }
}

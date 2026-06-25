using System.Text.Json.Serialization;

namespace GallopIQ.Api.DTOs;

public class RaceDto
{
    public string RaceId { get; set; } = string.Empty;
    public string CourseName { get; set; } = string.Empty;
    public string RaceName { get; set; } = string.Empty;
    public string OffTime { get; set; } = string.Empty;
    public string? Distance { get; set; }
    public string? Going { get; set; }
    public string? RaceClass { get; set; }
    public string? RegionCode { get; set; }
    public int RunnerCount { get; set; }
    public string Date { get; set; } = string.Empty;
}

public class RacecardRunnerDto
{
    public string HorseName { get; set; } = string.Empty;
    public string? Jockey { get; set; }
    public string? Trainer { get; set; }
    public string? Age { get; set; }
    public string? Form { get; set; }
    public int? Number { get; set; }
    public string? Odds { get; set; }
    public string? Lbs { get; set; }
}

public class RaceDetailDto : RaceDto
{
    public List<RacecardRunnerDto> Runners { get; set; } = new();
}

public class HorseFeatureDto
{
    public string HorseName { get; set; } = string.Empty;
    public string? Age { get; set; }
    public string? Form { get; set; }
    public string? Jockey { get; set; }
    public string? Trainer { get; set; }
    public string? WeightLbs { get; set; }
    public string? Odds { get; set; }
    public int RunnerNumber { get; set; }
}

public class PredictionDto
{
    public string HorseName { get; set; } = string.Empty;
    public double WinProbability { get; set; }
    public string Confidence { get; set; } = string.Empty;
    public double ValueScore { get; set; }
    public string Reasoning { get; set; } = string.Empty;
    public bool IsBestPick { get; set; }
}

public class RacePredictionResultDto
{
    public string RaceId { get; set; } = string.Empty;
    public DateTime GeneratedAt { get; set; }
    public List<PredictionDto> Horses { get; set; } = new();
}

public class GeminiPredictionItem
{
    [JsonPropertyName("horseName")]
    public string HorseName { get; set; } = string.Empty;

    [JsonPropertyName("winProbability")]
    public double WinProbability { get; set; }

    [JsonPropertyName("confidence")]
    public string Confidence { get; set; } = string.Empty;

    [JsonPropertyName("valueScore")]
    public double ValueScore { get; set; }

    [JsonPropertyName("reasoning")]
    public string Reasoning { get; set; } = string.Empty;
}

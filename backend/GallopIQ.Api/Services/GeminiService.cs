using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using GallopIQ.Api.DTOs;

namespace GallopIQ.Api.Services;

public class GeminiService : IGeminiService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _model;
    private readonly ILogger<GeminiService> _logger;

    public GeminiService(IHttpClientFactory factory, IConfiguration configuration, ILogger<GeminiService> logger)
    {
        _httpClient = factory.CreateClient("Gemini");
        _apiKey = configuration["Gemini:ApiKey"] ?? throw new InvalidOperationException("Gemini:ApiKey is not configured.");
        _model = configuration["Gemini:Model"] ?? "gemini-2.0-flash";
        _logger = logger;
    }

    public async Task<List<GeminiPredictionItem>> GetPredictionsAsync(RaceDetailDto race, List<HorseFeatureDto> features)
    {
        var featuresJson = JsonSerializer.Serialize(features);

        var promptText =
            $"You are an expert horse racing analyst. Analyse these runners for '{race.RaceName}' at {race.CourseName}. " +
            $"Distance: {race.Distance}, Going: {race.Going}.\n\nRunners:\n{featuresJson}\n\n" +
            $"Return ONLY a JSON array with no markdown. Each element: {{horseName, winProbability (0.0-1.0, all sum ~1.0), " +
            $"confidence (high/medium/low), valueScore (1.0-10.0), reasoning (brief string)}}. " +
            $"Include ALL {features.Count} runners.";

        var requestBody = new
        {
            contents = new[]
            {
                new
                {
                    role = "user",
                    parts = new[] { new { text = promptText } }
                }
            },
            generationConfig = new
            {
                response_mime_type = "application/json",
                temperature = 0.3
            }
        };

        var requestBytes = JsonSerializer.SerializeToUtf8Bytes(requestBody);
        var requestContent = new StringContent(
            Encoding.UTF8.GetString(requestBytes),
            Encoding.UTF8,
            "application/json");

        var url = $"/v1beta/models/{_model}:generateContent?key={_apiKey}";
        var response = await _httpClient.PostAsync(url, requestContent);

        var rawResponse = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Gemini API returned {StatusCode}. Raw response: {RawResponse}",
                response.StatusCode, rawResponse);
            throw new InvalidOperationException($"Gemini API error ({response.StatusCode}): {rawResponse}");
        }

        GeminiApiResponse? geminiResponse;
        try
        {
            geminiResponse = JsonSerializer.Deserialize<GeminiApiResponse>(rawResponse);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to deserialise Gemini API response. Raw: {RawResponse}", rawResponse);
            throw new InvalidOperationException("Failed to parse Gemini API response.", ex);
        }

        var text = geminiResponse?.Candidates?.FirstOrDefault()?.Content?.Parts?.FirstOrDefault()?.Text;

        if (string.IsNullOrWhiteSpace(text))
        {
            _logger.LogError("Gemini API returned empty text. Raw response: {RawResponse}", rawResponse);
            throw new InvalidOperationException("Gemini API returned an empty prediction.");
        }

        try
        {
            var predictions = JsonSerializer.Deserialize<List<GeminiPredictionItem>>(text);
            return predictions ?? new List<GeminiPredictionItem>();
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to deserialise Gemini prediction JSON. Text: {Text}", text);
            throw new InvalidOperationException("Failed to parse prediction data from Gemini response.", ex);
        }
    }

    // ── Private response shape ────────────────────────────────────────────────

    private class GeminiApiResponse
    {
        [JsonPropertyName("candidates")]
        public List<GeminiCandidate> Candidates { get; set; } = new();
    }

    private class GeminiCandidate
    {
        [JsonPropertyName("content")]
        public GeminiContent Content { get; set; } = new();
    }

    private class GeminiContent
    {
        [JsonPropertyName("parts")]
        public List<GeminiPart> Parts { get; set; } = new();
    }

    private class GeminiPart
    {
        [JsonPropertyName("text")]
        public string Text { get; set; } = string.Empty;
    }
}

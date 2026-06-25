using System.Text;
using GallopIQ.Api.Services;
using GallopIQ.Api.Stores;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// ── CORS ──────────────────────────────────────────────────────────────────────
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// ── HTTP CLIENTS ──────────────────────────────────────────────────────────────
var racingApiUsername = builder.Configuration["RacingApi:Username"] ?? "";
var racingApiPassword = builder.Configuration["RacingApi:Password"] ?? "";
var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{racingApiUsername}:{racingApiPassword}"));

builder.Services.AddHttpClient("RacingApi", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["RacingApi:BaseUrl"] ?? "https://api.theracingapi.com");
    client.DefaultRequestHeaders.Add(HeaderNames.Authorization, $"Basic {credentials}");
});

builder.Services.AddHttpClient("Gemini", client =>
{
    client.BaseAddress = new Uri("https://generativelanguage.googleapis.com");
});

// ── SERVICES ──────────────────────────────────────────────────────────────────
builder.Services.AddSingleton<PredictionStore>();
builder.Services.AddScoped<IRacingApiService, RacingApiService>();
builder.Services.AddScoped<IFeatureBuilderService, FeatureBuilderService>();
builder.Services.AddScoped<IGeminiService, GeminiService>();
builder.Services.AddScoped<IPredictionService, PredictionService>();

// ── MVC + JSON ────────────────────────────────────────────────────────────────
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DefaultIgnoreCondition =
            System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });

// ── SWAGGER ───────────────────────────────────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Gallop IQ API", Version = "v1" });
});

// ── PORT ──────────────────────────────────────────────────────────────────────
builder.WebHost.UseUrls("http://0.0.0.0:5000");

var app = builder.Build();

// ── MIDDLEWARE ────────────────────────────────────────────────────────────────
app.UseCors("AllowAngular");

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Gallop IQ API v1");
    c.RoutePrefix = "swagger";
});

app.MapControllers();

app.Run();

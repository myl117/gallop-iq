using GallopIQ.Api.DTOs;

namespace GallopIQ.Api.Services;

public class FeatureBuilderService : IFeatureBuilderService
{
    public List<HorseFeatureDto> BuildFeatures(RaceDetailDto race)
    {
        return race.Runners.Select(runner => new HorseFeatureDto
        {
            HorseName = runner.HorseName,
            Age = runner.Age,
            Form = runner.Form,
            Jockey = runner.Jockey,
            Trainer = runner.Trainer,
            WeightLbs = runner.Lbs,
            Odds = runner.Odds,
            RunnerNumber = runner.Number ?? 0
        }).ToList();
    }
}

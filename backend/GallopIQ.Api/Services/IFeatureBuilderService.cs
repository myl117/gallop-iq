using GallopIQ.Api.DTOs;

namespace GallopIQ.Api.Services;

public interface IFeatureBuilderService
{
    List<HorseFeatureDto> BuildFeatures(RaceDetailDto race);
}

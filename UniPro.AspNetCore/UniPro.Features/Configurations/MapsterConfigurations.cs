using Mapster;
using UniPro.Domain.Entities;
using UniPro.Features.Common.Responses;

namespace UniPro.Features.Configurations;

public static class MapsterConfigurations
{
    public static void Configure()
    {
        TypeAdapterConfig<University, UniversityResponse>
            .ForType()
            .Map(dest => dest.UniversityName, src => src.Name);
        
        TypeAdapterConfig<Academic, AcademicResponse>
            .ForType()
            .Map(dest => dest.AcademicName, src => src.Name);
    }
}
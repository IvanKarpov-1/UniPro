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
        
        TypeAdapterConfig<Department, DepartmentResponse>
            .ForType()
            .Map(dest => dest.DepartmentName, src => src.Name);

        TypeAdapterConfig<StudentGroup, StudentGroupResponse>
            .ForType()
            .Map(dest => dest.StudentGroupName, src => src.Name)
            .Map(dest => dest.Students, src => src.StudentInfos);

        TypeAdapterConfig<User, UserResponse>
            .ForType()
            .MapToConstructor(true);
    }
}
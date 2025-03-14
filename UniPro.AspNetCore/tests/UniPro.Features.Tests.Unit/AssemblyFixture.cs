using Mapster;
using UniPro.Domain.Entities;
using UniPro.Features.Common.Responses;
using UniPro.Features.Features.Users;
using Xunit.Abstractions;
using Xunit.Sdk;

[assembly: TestFramework("UniPro.Features.Tests.Unit.AssemblyFixture", "UniPro.Features.Tests.Unit")]

namespace UniPro.Features.Tests.Unit;

public sealed class AssemblyFixture : XunitTestFramework
{
    public AssemblyFixture(IMessageSink messageSink) : base(messageSink)
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
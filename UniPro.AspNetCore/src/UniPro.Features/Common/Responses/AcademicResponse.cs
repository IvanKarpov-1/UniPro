namespace UniPro.Features.Common.Responses;

public sealed record AcademicResponse(
    int AcademicId,
    int UniversityId,
    string AcademicName,
    List<DepartmentResponse>? Departments);
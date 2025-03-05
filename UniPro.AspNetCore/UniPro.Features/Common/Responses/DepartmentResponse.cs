namespace UniPro.Features.Common.Responses;

public sealed record DepartmentResponse(
    int DepartmentId,
    int AcademicId,
    string DepartmentName);
using UniPro.Domain.Entities;

namespace UniPro.Features.Common.Responses;

public sealed record StudentGroupResponse(
    int StudentGroupId,
    int DepartmentId,
    string StudentGroupName,
    List<StudentInfoResponse> Students);
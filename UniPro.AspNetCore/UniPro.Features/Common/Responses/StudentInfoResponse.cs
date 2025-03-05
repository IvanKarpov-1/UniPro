namespace UniPro.Features.Common.Responses;

public sealed record StudentInfoResponse(
    string StudentId,
    string FirstName,
    string LastName,
    string Patronymic,
    string Avatar,
    string PhoneNumber,
    string UniversityName,
    string AcademicName,
    string DepartmentName,
    string StudentGroupName);
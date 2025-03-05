namespace UniPro.Features.Common.Responses;

public sealed record TeacherInfoResponse(
    string TeacherId,
    string FirstName,
    string LastName,
    string Patronymic,
    string Avatar,
    string PhoneNumber,
    string UniversityName,
    string AcademicName,
    string DepartmentName);
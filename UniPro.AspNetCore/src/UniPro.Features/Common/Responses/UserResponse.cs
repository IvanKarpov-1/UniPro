namespace UniPro.Features.Common.Responses;

public sealed record UserResponse(
    string UserId,
    string FirstName,
    string LastName,
    string Patronymic,
    string Avatar,
    string PhoneNumber)
{
    public string? UserRole { get; set; }
    
    public string? UniversityName { get; set; }

    public string? AcademicName { get; set; }
    
    public string? DepartmentName { get; set; }
    
    public string? StudentGroupName { get; set; }
}
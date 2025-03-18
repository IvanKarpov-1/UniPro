using UniPro.Domain.Entities;
using UniPro.Domain.Entities.SuperTokens;
using UniPro.Features.Common.Responses;
using UniPro.Features.Tests.Integration.Setup;
using UniPro.Infrastructure.Database;
using Task = System.Threading.Tasks.Task;

namespace UniPro.Features.Tests.Integration.Users;

[Collection(nameof(UniProWebApplicationFactory))]
public class GetUserTests(UniProWebApplicationFactory factory) : IAsyncLifetime
{
    private readonly HttpClient _client = factory.HttpClient;
    private readonly UniProDbContext _dbContext = factory.DbContext;
    private readonly Func<Task> _resetDatabase = factory.ResetDatabaseAsync;
    private readonly AutoFaker _autoFaker = new();

    [Fact]
    public async Task Get_WhenNoUsersInDb_ReturnsNotFoundProblem()
    {
        // Arrange
        var userId = _autoFaker.Generate<Guid>().ToString();

        // Act
        var response = await _client.GetAsync($"/api/users/{userId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Get_WhenUsersInDbButWithoutUserWithProvidedId_ReturnsNotFoundProblem()
    {
        // Arrange
        await _dbContext.PopulateUsers(1);
        var userId = _autoFaker.Generate<Guid>().ToString();

        // Act
        var response = await _client.GetAsync($"/api/users/{userId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Get_WhenUserWithProvidedId_ReturnsUserResponse()
    {
        // Arrange
        var dbUser = (await _dbContext.PopulateUsers(1))[0];
        var userId = dbUser.UserId;

        var userResponse = new UserResponse
        (
            dbUser.UserId,
            dbUser.FirstName,
            dbUser.LastName,
            dbUser.Patronymic,
            dbUser.Avatar!,
            dbUser.PhoneNumber
        );

        // Act
        var response = await _client.GetAsync($"/api/users/{userId}");
        var user = await response.Content.ReadFromJsonAsync<UserResponse>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        user.Should().NotBeNull();
        user.Should().BeEquivalentTo(userResponse);
    }

    [Theory]
    [InlineData("admin")]
    [InlineData("teacher")]
    [InlineData("student")]
    public async Task Get_WhenUserWithProvidedIdAndRole_ReturnsUserResponse(string role)
    {
        // Arrange
        var dbUser = (await _dbContext.PopulateUsers(1))[0];
        var userId = dbUser.UserId;

        var userRole = new StUserRole
        {
            AppId = "public",
            UserId = dbUser.UserId,
            Role = role,
            TenantId = ""
        };

        _dbContext.StUserRoles.Add(userRole);

        var populateUniversityInfoResult = await _dbContext.PopulateUniversityInfo();

        var teacherInfo = new Faker<TeacherInfo>()
            .RuleFor(t => t.TeacherId, dbUser.UserId)
            .RuleFor(t => t.UniversityId, populateUniversityInfoResult.University.UniversityId)
            .RuleFor(t => t.AcademicId, populateUniversityInfoResult.Academic.AcademicId)
            .RuleFor(t => t.DepartmentId, populateUniversityInfoResult.Department.DepartmentId)
            .Generate();
        var studentInfo = new Faker<StudentInfo>()
            .RuleFor(s => s.StudentId, dbUser.UserId)
            .RuleFor(t => t.UniversityId, populateUniversityInfoResult.University.UniversityId)
            .RuleFor(t => t.AcademicId, populateUniversityInfoResult.Academic.AcademicId)
            .RuleFor(t => t.DepartmentId, populateUniversityInfoResult.Department.DepartmentId)
            .RuleFor(t => t.StudentGroupId, populateUniversityInfoResult.StudentGroup.StudentGroupId)
            .Generate();

        _dbContext.TeacherInfos.Add(teacherInfo);
        _dbContext.StudentInfos.Add(studentInfo);

        await _dbContext.SaveChangesAsync();

        var userResponse = new UserResponse
        (
            dbUser.UserId,
            dbUser.FirstName,
            dbUser.LastName,
            dbUser.Patronymic,
            dbUser.Avatar!,
            dbUser.PhoneNumber
        )
        {
            UserRole = role,
        };

        if (role != "admin")
        {
            userResponse.UniversityName = populateUniversityInfoResult.University.Name;
            userResponse.AcademicName = populateUniversityInfoResult.Academic.Name;
            userResponse.DepartmentName = populateUniversityInfoResult.Department.Name;
            userResponse.StudentGroupName = role == "student"
                ? populateUniversityInfoResult.StudentGroup.Name
                : null;
        }

        // Act
        var response = await _client.GetAsync($"/api/users/{userId}");
        var user = await response.Content.ReadFromJsonAsync<UserResponse>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        user.Should().NotBeNull();
        user.Should().BeEquivalentTo(userResponse);
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync() => _resetDatabase();
}
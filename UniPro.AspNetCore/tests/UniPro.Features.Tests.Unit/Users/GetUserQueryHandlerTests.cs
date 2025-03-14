using UniPro.Domain.Entities;
using UniPro.Domain.Entities.SuperTokens;
using UniPro.Features.Features.Users;
using UniPro.Infrastructure.Database;
using Task = System.Threading.Tasks.Task;

namespace UniPro.Features.Tests.Unit.Users;

public class GetUserQueryHandlerTests
{
    private readonly UniProDbContext _dbContext = Create.MockedDbContextFor<UniProDbContext>();
    private readonly AutoFaker _autoFaker = new();

    [Fact]
    public async Task Handle_WhenNoUserInDb_ReturnFailResultWithNotFoundError()
    {
        // Arrange
        var userId = _autoFaker.Generate<Guid>().ToString();
        var query = new GetUserQuery(userId);
        var handler = new GetUserQueryHandler(_dbContext);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Message.Should().Be($"User with ID {userId} not found.");
    }

    [Fact]
    public async Task Handle_WhenNoUserWithProvidedId_ReturnFailResultWithNotFoundError()
    {
        // Arrange
        var faker = new Faker<User>();
        var dbUser = faker
            .RuleFor(u => u.UserId, _autoFaker.Generate<Guid>().ToString())
            .RuleFor(u => u.AppId, "public")
            .RuleFor(u => u.FirstName, f => f.Name.FirstName())
            .RuleFor(u => u.LastName, f => f.Name.LastName())
            .RuleFor(u => u.Patronymic, f => f.Name.FullName())
            .RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber())
            .Generate();
        _dbContext.Set<User>().Add(dbUser);
        await _dbContext.SaveChangesAsync();

        var userId = _autoFaker.Generate<Guid>().ToString();
        var query = new GetUserQuery(userId);
        var handler = new GetUserQueryHandler(_dbContext);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Message.Should().Be($"User with ID {userId} not found.");
    }

    [Fact]
    public async Task Handle_WhenUserWithProvidedId_ReturnSuccessResultWithUserResponse()
    {
        // Arrange
        var faker = new Faker<User>();
        var dbUser = faker
            .RuleFor(u => u.UserId, _autoFaker.Generate<Guid>().ToString())
            .RuleFor(u => u.AppId, "public")
            .RuleFor(u => u.FirstName, f => f.Name.FirstName())
            .RuleFor(u => u.LastName, f => f.Name.LastName())
            .RuleFor(u => u.Patronymic, f => f.Name.FullName())
            .RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber())
            .RuleFor(u => u.Avatar, f => f.Internet.Avatar())
            .Generate();
        _dbContext.Set<User>().Add(dbUser);
        await _dbContext.SaveChangesAsync();

        var userResponse = new UserResponse
        (
            dbUser.UserId,
            dbUser.FirstName,
            dbUser.LastName,
            dbUser.Patronymic,
            dbUser.Avatar!,
            dbUser.PhoneNumber
        );

        var userId = dbUser.UserId;
        var query = new GetUserQuery(userId);
        var handler = new GetUserQueryHandler(_dbContext);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(userResponse);
    }

    [Theory]
    [InlineData("admin")]
    [InlineData("teacher")]
    [InlineData("student")]
    public async Task Handle_WhenUserWithProvidedIdAndHaveRole_ReturnSuccessResultWithUserResponse(string role)
    {
        // Arrange
        var faker = new Faker<User>();
        var dbUser = faker
            .RuleFor(u => u.UserId, _autoFaker.Generate<Guid>().ToString())
            .RuleFor(u => u.AppId, "public")
            .RuleFor(u => u.FirstName, f => f.Name.FirstName())
            .RuleFor(u => u.LastName, f => f.Name.LastName())
            .RuleFor(u => u.Patronymic, f => f.Name.FullName())
            .RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber())
            .RuleFor(u => u.Avatar, f => f.Internet.Avatar())
            .Generate();
        _dbContext.Set<User>().Add(dbUser);

        var userRole = new StUserRole
        {
            AppId = "public",
            UserId = dbUser.UserId,
            Role = role,
            TenantId = ""
        };

        _dbContext.Set<StUserRole>().Add(userRole);

        var university = new Faker<University>()
            .RuleFor(u => u.UniversityId, _autoFaker.Generate<int>())
            .RuleFor(u => u.Name, f => f.Company.CompanyName())
            .Generate();
        var academic = new Faker<Academic>()
            .RuleFor(u => u.AcademicId, _autoFaker.Generate<int>())
            .RuleFor(u => u.Name, f => f.Company.CompanyName())
            .Generate();
        var department = new Faker<Department>()
            .RuleFor(u => u.DepartmentId, _autoFaker.Generate<int>())
            .RuleFor(u => u.Name, f => f.Company.CompanyName())
            .Generate();
        var studentGroup = new Faker<StudentGroup>()
            .RuleFor(u => u.StudentGroupId, _autoFaker.Generate<int>())
            .RuleFor(u => u.Name, f => f.Company.CompanyName())
            .Generate();

        var teacherInfo = new Faker<TeacherInfo>()
            .RuleFor(t => t.TeacherId, dbUser.UserId)
            .RuleFor(t => t.UniversityId, university.UniversityId)
            .RuleFor(t => t.University, university)
            .RuleFor(t => t.AcademicId, academic.AcademicId)
            .RuleFor(t => t.Academic, academic)
            .RuleFor(t => t.DepartmentId, department.DepartmentId)
            .RuleFor(t => t.Department, department)
            .Generate();
        var studentInfo = new Faker<StudentInfo>()
            .RuleFor(s => s.StudentId, dbUser.UserId)
            .RuleFor(t => t.UniversityId, university.UniversityId)
            .RuleFor(t => t.University, university)
            .RuleFor(t => t.AcademicId, academic.AcademicId)
            .RuleFor(t => t.Academic, academic)
            .RuleFor(t => t.DepartmentId, department.DepartmentId)
            .RuleFor(t => t.Department, department)
            .RuleFor(t => t.StudentGroupId, studentGroup.StudentGroupId)
            .RuleFor(t => t.StudentGroup, studentGroup)
            .Generate();

        _dbContext.Set<TeacherInfo>().Add(teacherInfo);
        _dbContext.Set<StudentInfo>().Add(studentInfo);

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
            userResponse.UniversityName = university.Name;
            userResponse.AcademicName = academic.Name;
            userResponse.DepartmentName = department.Name;
            userResponse.StudentGroupName = role == "student" ? studentGroup.Name : null;
        }

        var userId = dbUser.UserId;
        var query = new GetUserQuery(userId);
        var handler = new GetUserQueryHandler(_dbContext);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(userResponse);
    }
}
using UniPro.Domain.Entities;
using UniPro.Features.Common.Responses;
using UniPro.Features.Features.UniversityInfo.StudentGroups;
using UniPro.Infrastructure.Database;
using Task = System.Threading.Tasks.Task;

namespace UniPro.Features.Tests.Unit.UniversityInfo.StudentGroups;

public class AddStudentGroupCommandHandlerTests
{
    private readonly UniProDbContext _dbContext = Create.MockedDbContextFor<UniProDbContext>();
    private readonly AutoFaker _autoFaker = new();

    [Fact]
    public async Task Handle_WhenNoDepartmentWithId_ReturnFailResultWithNotFoundError()
    {
        // Arrange
        var departmentId = _autoFaker.Generate<int>();
        var command = new AddStudentGroupCommand(departmentId, "");
        var handler = new AddStudentGroupCommandHandler(_dbContext);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Message.Should().Be($"Department with ID {departmentId} not found.");
    }

    [Fact]
    public async Task Handle_WhenStudentGroupExists_ReturnFailResultWithAlreadyExistError()
    {
        // Arrange
        var departmentFaker = new Faker<Department>();
        var dbDepartment = departmentFaker
            .RuleFor(a => a.AcademicId, _autoFaker.Generate<int>())
            .RuleFor(a => a.Name, f => f.Company.CompanyName())
            .Generate();
        
        var studentGroupName = _autoFaker.Generate<string>();
        
        var studentGroupFaker = new Faker<StudentGroup>();
        var dbStudentGroup = studentGroupFaker
            .RuleFor(u => u.StudentGroupId, _autoFaker.Generate<int>())
            .RuleFor(u => u.DepartmentId, dbDepartment.DepartmentId)
            .RuleFor(a => a.Name, studentGroupName)
            .Generate();
        
        _dbContext.Departments.Add(dbDepartment);
        _dbContext.StudentGroups.Add(dbStudentGroup);
        await _dbContext.SaveChangesAsync();
        
        var command = new AddStudentGroupCommand(dbDepartment.DepartmentId, studentGroupName);
        var handler = new AddStudentGroupCommandHandler(_dbContext);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Message.Should().Be($"Student group with name \"{studentGroupName}\" already exists.");
    }

    [Fact]
    public async Task Handle_WhenStudentGroupNotExists_ReturnSuccessResultWithStudentGroupResponse()
    {
        // Arrange
        var departmentFaker = new Faker<Department>();
        var dbDepartment = departmentFaker
            .RuleFor(a => a.AcademicId, _autoFaker.Generate<int>())
            .RuleFor(a => a.Name, f => f.Company.CompanyName())
            .Generate();
        
        var studentGroupFaker = new Faker<StudentGroup>();
        var dbStudentGroup = studentGroupFaker
            .RuleFor(u => u.DepartmentId, dbDepartment.DepartmentId)
            .Generate();
        
        _dbContext.Departments.Add(dbDepartment);
        await _dbContext.SaveChangesAsync();

        var studentGroupName = _autoFaker.Generate<string>();
        
        var studentGroupResponse = new StudentGroupResponse(
            1,
            dbDepartment.DepartmentId,
            studentGroupName,
            []);
        
        var command = new AddStudentGroupCommand(dbDepartment.DepartmentId, studentGroupName);
        var handler = new AddStudentGroupCommandHandler(_dbContext);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(studentGroupResponse);
    }
}
using UniPro.Domain.Entities;
using UniPro.Features.Features.UniversityInfo.StudentGroups;
using UniPro.Infrastructure.Database;
using Task = System.Threading.Tasks.Task;

namespace UniPro.Features.Tests.Unit.UniversityInfo.StudentGroups;

public class DeleteStudentGroupCommandHandlerTests
{
    private readonly UniProDbContext _dbContext = Create.MockedDbContextFor<UniProDbContext>();
    private readonly AutoFaker _autoFaker = new();

    [Fact]
    public async Task Handle_WhenNoStudentGroupInDb_ReturnFailResultWithNotFoundError()
    {
        // Arrange
        var studentGroupId = _autoFaker.Generate<int>();
        var command = new DeleteStudentGroupCommand(studentGroupId);
        var handler = new DeleteStudentGroupCommandHandler(_dbContext);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Message.Should().Be($"Student group with ID {studentGroupId} not found.");
    }

    [Fact]
    public async Task Handle_WhenNoStudentGroupWithProvidedId_ReturnFailResultWithNotFoundError()
    {
        // Arrange
        var faker = new Faker<StudentGroup>();
        var dbStudentGroup = faker
            .RuleFor(a => a.DepartmentId, _autoFaker.Generate<int>())
            .RuleFor(a => a.Name, f => f.Company.CompanyName())
            .Generate();
        
        _dbContext.StudentGroups.Add(dbStudentGroup);
        await _dbContext.SaveChangesAsync();
        
        var studentGroupId = _autoFaker.Generate<int>();
        var command = new DeleteStudentGroupCommand(studentGroupId);
        var handler = new DeleteStudentGroupCommandHandler(_dbContext);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Message.Should().Be($"Student group with ID {studentGroupId} not found.");
    }

    [Fact]
    public async Task Handle_WhenStudentGroupWithProvidedId_ReturnSuccessResult()
    {
        // Arrange
        var faker = new Faker<StudentGroup>();
        var dbStudentGroup = faker
            .RuleFor(a => a.DepartmentId, _autoFaker.Generate<int>())
            .RuleFor(a => a.Name, f => f.Company.CompanyName())
            .Generate();
        
        _dbContext.StudentGroups.Add(dbStudentGroup);
        await _dbContext.SaveChangesAsync();

        var studentGroupId = dbStudentGroup.StudentGroupId;
        var command = new DeleteStudentGroupCommand(studentGroupId);
        var handler = new DeleteStudentGroupCommandHandler(_dbContext);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        var studentGroup = await _dbContext.StudentGroups.FindAsync(studentGroupId);
        
        result.IsSuccess.Should().BeTrue();
        studentGroup.Should().BeNull();
    }
}
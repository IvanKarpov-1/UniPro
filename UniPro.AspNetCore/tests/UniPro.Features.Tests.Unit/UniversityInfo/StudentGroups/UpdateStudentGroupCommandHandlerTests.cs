using UniPro.Domain.Entities;
using UniPro.Features.Features.UniversityInfo.StudentGroups;
using UniPro.Infrastructure.Database;
using Task = System.Threading.Tasks.Task;

namespace UniPro.Features.Tests.Unit.UniversityInfo.StudentGroups;

public class UpdateStudentGroupCommandHandlerTests
{
    private readonly UniProDbContext _dbContext = Create.MockedDbContextFor<UniProDbContext>();
    private readonly AutoFaker _autoFaker = new();
    
    [Fact]
    public async Task Handle_WhenNoStudentGroupInDb_ReturnFailResultWithNotFoundError()
    {
        // Arrange
        var studentGroupId = _autoFaker.Generate<int>();
        var command = new UpdateStudentGroupCommand(
            studentGroupId,
            _autoFaker.Generate<string>(),
            null);
        var handler = new UpdateStudentGroupCommandHandler(_dbContext);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Message.Should().Be($"Student group with ID {studentGroupId} not found.");
    }
    
    [Theory]
    [InlineData("StudentGroup name", null)]
    [InlineData("StudentGroup name", 2)]
    public async Task Handle_WhenStudentGroupWithProvidedId_ReturnSuccessResult(string newStudentGroupName, int? newDepartmentId)
    {
        // Arrange
        var faker = new Faker<StudentGroup>();
        var dbStudentGroup = faker
            .RuleFor(a => a.StudentGroupId, _autoFaker.Generate<int>())
            .RuleFor(a => a.DepartmentId, _autoFaker.Generate<int>())
            .RuleFor(a => a.Name, f => f.Company.CompanyName())
            .Generate();
        _dbContext.StudentGroups.Add(dbStudentGroup);
        await _dbContext.SaveChangesAsync();
        
        var command = new UpdateStudentGroupCommand(dbStudentGroup.StudentGroupId,
            newStudentGroupName,
            newDepartmentId);

        var handler = new UpdateStudentGroupCommandHandler(_dbContext);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        var updatedStudentGroup = await _dbContext.StudentGroups.FindAsync(dbStudentGroup.StudentGroupId);
        
        result.IsSuccess.Should().BeTrue();
        updatedStudentGroup!.Name.Should().Be(newStudentGroupName);
        updatedStudentGroup!.DepartmentId.Should().Be(newDepartmentId ?? dbStudentGroup.DepartmentId);
    }
}
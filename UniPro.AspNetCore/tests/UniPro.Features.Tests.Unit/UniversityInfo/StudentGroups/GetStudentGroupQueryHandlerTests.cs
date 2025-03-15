using UniPro.Domain.Entities;
using UniPro.Features.Common.Responses;
using UniPro.Features.Features.UniversityInfo.StudentGroups;
using UniPro.Infrastructure.Database;
using Task = System.Threading.Tasks.Task;

namespace UniPro.Features.Tests.Unit.UniversityInfo.StudentGroups;

public class GetStudentGroupQueryHandlerTests
{
    private readonly UniProDbContext _dbContext = Create.MockedDbContextFor<UniProDbContext>();
    private readonly AutoFaker _autoFaker = new();

    [Fact]
    public async Task Handle_WhenNoStudentGroupInDb_ReturnFailResultWithNotFoundError()
    {
        // Arrange
        var studentGroupId = _autoFaker.Generate<int>();
        var query = new GetStudentGroupQuery(studentGroupId);
        var handler = new GetStudentGroupQueryHandler(_dbContext);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

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
        var query = new GetStudentGroupQuery(studentGroupId);
        var handler = new GetStudentGroupQueryHandler(_dbContext);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Message.Should().Be($"Student group with ID {studentGroupId} not found.");
    }

    [Fact]
    public async Task Handle_WhenStudentGroupWithProvidedId_ReturnSuccessResultWithStudentGroupResponse()
    {
        // Arrange
        var faker = new Faker<StudentGroup>();
        var dbStudentGroup = faker
            .RuleFor(a => a.StudentGroupId, _autoFaker.Generate<int>())
            .RuleFor(a => a.DepartmentId, _autoFaker.Generate<int>())
            .RuleFor(a => a.Name, f => f.Company.CompanyName())
            .RuleFor(a => a.StudentInfos, new List<StudentInfo>())
            .Generate();
        _dbContext.StudentGroups.Add(dbStudentGroup);
        await _dbContext.SaveChangesAsync();

        var studentGroupId = dbStudentGroup.StudentGroupId;
        var query = new GetStudentGroupQuery(studentGroupId);
        var handler = new GetStudentGroupQueryHandler(_dbContext);

        var studentGroupResponse = new StudentGroupResponse(
            studentGroupId,
            dbStudentGroup.DepartmentId,
            dbStudentGroup.Name,
            []);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(studentGroupResponse);
    }
}
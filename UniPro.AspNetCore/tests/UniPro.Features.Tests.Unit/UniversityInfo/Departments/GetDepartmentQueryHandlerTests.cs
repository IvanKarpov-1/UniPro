using UniPro.Domain.Entities;
using UniPro.Features.Common.Responses;
using UniPro.Features.Features.UniversityInfo.Departments;
using UniPro.Infrastructure.Database;
using Task = System.Threading.Tasks.Task;

namespace UniPro.Features.Tests.Unit.UniversityInfo.Departments;

public class GetDepartmentQueryHandlerTests
{
    private readonly UniProDbContext _dbContext = Create.MockedDbContextFor<UniProDbContext>();
    private readonly AutoFaker _autoFaker = new();

    [Fact]
    public async Task Handle_WhenNoDepartmentInDb_ReturnFailResultWithNotFoundError()
    {
        // Arrange
        var departmentId = _autoFaker.Generate<int>();
        var query = new GetDepartmentQuery(departmentId);
        var handler = new GetDepartmentQueryHandler(_dbContext);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Message.Should().Be($"Department with ID {departmentId} not found.");
    }

    [Fact]
    public async Task Handle_WhenNoDepartmentWithProvidedId_ReturnFailResultWithNotFoundError()
    {
        // Arrange
        var faker = new Faker<Department>();
        var dbDepartment = faker
            .RuleFor(a => a.DepartmentId, _autoFaker.Generate<int>())
            .RuleFor(a => a.AcademicId, _autoFaker.Generate<int>())
            .RuleFor(a => a.Name, f => f.Company.CompanyName())
            .Generate();
        _dbContext.Departments.Add(dbDepartment);
        await _dbContext.SaveChangesAsync();
        
        var departmentId = _autoFaker.Generate<int>();
        var query = new GetDepartmentQuery(departmentId);
        var handler = new GetDepartmentQueryHandler(_dbContext);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Message.Should().Be($"Department with ID {departmentId} not found.");
    }

    [Fact]
    public async Task Handle_WhenDepartmentWithProvidedId_ReturnSuccessResultWithDepartmentResponse()
    {
        // Arrange
        var faker = new Faker<Department>();
        var dbDepartment = faker
            .RuleFor(a => a.AcademicId, _autoFaker.Generate<int>())
            .RuleFor(a => a.Name, f => f.Company.CompanyName())
            .RuleFor(a => a.StudentGroups, new List<StudentGroup>())
            .Generate();
        _dbContext.Departments.Add(dbDepartment);
        await _dbContext.SaveChangesAsync();

        var departmentId = dbDepartment.DepartmentId;
        var query = new GetDepartmentQuery(departmentId);
        var handler = new GetDepartmentQueryHandler(_dbContext);

        var departmentResponse = new DepartmentResponse(
            departmentId,
            dbDepartment.AcademicId,
            dbDepartment.Name,
            []);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(departmentResponse);
    }
}
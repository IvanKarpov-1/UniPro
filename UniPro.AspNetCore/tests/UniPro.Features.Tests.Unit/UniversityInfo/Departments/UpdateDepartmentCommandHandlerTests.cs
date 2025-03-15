using UniPro.Domain.Entities;
using UniPro.Features.Features.UniversityInfo.Departments;
using UniPro.Infrastructure.Database;
using Task = System.Threading.Tasks.Task;

namespace UniPro.Features.Tests.Unit.UniversityInfo.Departments;

public class UpdateDepartmentCommandHandlerTests
{
    private readonly UniProDbContext _dbContext = Create.MockedDbContextFor<UniProDbContext>();
    private readonly AutoFaker _autoFaker = new();
    
    [Fact]
    public async Task Handle_WhenNoDepartmentInDb_ReturnFailResultWithNotFoundError()
    {
        // Arrange
        var departmentId = _autoFaker.Generate<int>();
        var command = new UpdateDepartmentCommand(
            departmentId,
            _autoFaker.Generate<string>(),
            null);
        var handler = new UpdateDepartmentCommandHandler(_dbContext);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Message.Should().Be($"Department with ID {departmentId} not found.");
    }
    
    [Theory]
    [InlineData("Department name", null)]
    [InlineData("Department name", 2)]
    public async Task Handle_WhenDepartmentWithProvidedId_ReturnSuccessResult(string newDepartmentName, int? newAcademicId)
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
        
        var command = new UpdateDepartmentCommand(
            dbDepartment.DepartmentId,
            newDepartmentName,
            newAcademicId);

        var handler = new UpdateDepartmentCommandHandler(_dbContext);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        var updatedDepartment = await _dbContext.Departments.FindAsync(dbDepartment.DepartmentId);
        
        result.IsSuccess.Should().BeTrue();
        updatedDepartment!.Name.Should().Be(newDepartmentName);
        updatedDepartment!.AcademicId.Should().Be(newAcademicId ?? dbDepartment.AcademicId);
    }
}
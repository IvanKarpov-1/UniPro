using UniPro.Domain.Entities;
using UniPro.Features.Features.UniversityInfo.Departments;
using UniPro.Infrastructure.Database;
using Task = System.Threading.Tasks.Task;

namespace UniPro.Features.Tests.Unit.UniversityInfo.Departments;

public class DeleteDepartmentCommandHandlerTests
{
    private readonly UniProDbContext _dbContext = Create.MockedDbContextFor<UniProDbContext>();
    private readonly AutoFaker _autoFaker = new();

    [Fact]
    public async Task Handle_WhenNoDepartmentInDb_ReturnFailResultWithNotFoundError()
    {
        // Arrange
        var departmentId = _autoFaker.Generate<int>();
        var command = new DeleteDepartmentCommand(departmentId);
        var handler = new DeleteDepartmentCommandHandler(_dbContext);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

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
            .RuleFor(a => a.AcademicId, _autoFaker.Generate<int>())
            .RuleFor(a => a.Name, f => f.Company.CompanyName())
            .Generate();
        
        _dbContext.Departments.Add(dbDepartment);
        await _dbContext.SaveChangesAsync();
        
        var departmentId = _autoFaker.Generate<int>();
        var command = new DeleteDepartmentCommand(departmentId);
        var handler = new DeleteDepartmentCommandHandler(_dbContext);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Message.Should().Be($"Department with ID {departmentId} not found.");
    }

    [Fact]
    public async Task Handle_WhenDepartmentWithProvidedId_ReturnSuccessResult()
    {
        // Arrange
        var faker = new Faker<Department>();
        var dbDepartment = faker
            .RuleFor(a => a.AcademicId, _autoFaker.Generate<int>())
            .RuleFor(a => a.Name, f => f.Company.CompanyName())
            .Generate();
        
        _dbContext.Departments.Add(dbDepartment);
        await _dbContext.SaveChangesAsync();

        var departmentId = dbDepartment.DepartmentId;
        var command = new DeleteDepartmentCommand(departmentId);
        var handler = new DeleteDepartmentCommandHandler(_dbContext);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        var department = await _dbContext.Departments.FindAsync(departmentId);
        
        result.IsSuccess.Should().BeTrue();
        department.Should().BeNull();
    }
}
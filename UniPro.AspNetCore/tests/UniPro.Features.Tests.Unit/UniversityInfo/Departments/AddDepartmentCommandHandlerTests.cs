using UniPro.Domain.Entities;
using UniPro.Features.Common.Responses;
using UniPro.Features.Features.UniversityInfo.Departments;
using UniPro.Infrastructure.Database;
using Task = System.Threading.Tasks.Task;

namespace UniPro.Features.Tests.Unit.UniversityInfo.Departments;

public class AddDepartmentCommandHandlerTests
{
    private readonly UniProDbContext _dbContext = Create.MockedDbContextFor<UniProDbContext>();
    private readonly AutoFaker _autoFaker = new();

    [Fact]
    public async Task Handle_WhenNoUniversityWithId_ReturnFailResultWithNotFoundError()
    {
        // Arrange
        var academicId = _autoFaker.Generate<int>();
        var command = new AddDepartmentCommand(academicId, "");
        var handler = new AddDepartmentCommandHandler(_dbContext);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Message.Should().Be($"Academic with ID {academicId} not found.");
    }

    [Fact]
    public async Task Handle_WhenDepartmentExists_ReturnFailResultWithAlreadyExistError()
    {
        // Arrange
        var academicFaker = new Faker<Academic>();
        var dbAcademic = academicFaker
            .RuleFor(a => a.UniversityId, _autoFaker.Generate<int>())
            .RuleFor(a => a.Name, f => f.Company.CompanyName())
            .Generate();
        
        var departmentName = _autoFaker.Generate<string>();
        
        var departmentFaker = new Faker<Department>();
        var dbDepartment = departmentFaker
            .RuleFor(u => u.AcademicId, dbAcademic.AcademicId)
            .RuleFor(a => a.Name, departmentName)
            .Generate();
        
        _dbContext.Academics.Add(dbAcademic);
        _dbContext.Departments.Add(dbDepartment);
        await _dbContext.SaveChangesAsync();
        
        var command = new AddDepartmentCommand(dbAcademic.AcademicId, departmentName);
        var handler = new AddDepartmentCommandHandler(_dbContext);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Message.Should().Be($"Department with name \"{departmentName}\" already exists.");
    }

    [Fact]
    public async Task Handle_WhenDepartmentNotExists_ReturnSuccessResultWithDepartmentResponse()
    {
        // Arrange
        var academicFaker = new Faker<Academic>();
        var dbAcademic = academicFaker
            .RuleFor(a => a.UniversityId, _autoFaker.Generate<int>())
            .RuleFor(a => a.Name, f => f.Company.CompanyName())
            .Generate();
        
        _dbContext.Academics.Add(dbAcademic);
        await _dbContext.SaveChangesAsync();
        
        var departmentName = _autoFaker.Generate<string>();
        
        var departmentResponse = new DepartmentResponse(
            1,
            dbAcademic.AcademicId,
            departmentName,
            []);
        
        var command = new AddDepartmentCommand(dbAcademic.AcademicId, departmentName);
        var handler = new AddDepartmentCommandHandler(_dbContext);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(departmentResponse);
    }
}
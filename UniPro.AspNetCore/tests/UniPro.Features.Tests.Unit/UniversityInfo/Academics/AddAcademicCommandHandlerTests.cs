using UniPro.Domain.Entities;
using UniPro.Features.Common.Responses;
using UniPro.Features.Features.UniversityInfo.Academics;
using UniPro.Infrastructure.Database;
using Task = System.Threading.Tasks.Task;

namespace UniPro.Features.Tests.Unit.UniversityInfo.Academics;

public class AddAcademicCommandHandlerTests
{
    private readonly UniProDbContext _dbContext = Create.MockedDbContextFor<UniProDbContext>();
    private readonly AutoFaker _autoFaker = new();

    [Fact]
    public async Task Handle_WhenNoUniversityWithId_ReturnFailResultWithNotFoundError()
    {
        // Arrange
        var universityId = _autoFaker.Generate<int>();
        var command = new AddAcademicCommand(universityId, "");
        var handler = new AddAcademicCommandHandler(_dbContext);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Message.Should().Be($"University with ID {universityId} not found.");
    }

    [Fact]
    public async Task Handle_WhenAcademicExists_ReturnFailResultWithAlreadyExistError()
    {
        // Arrange
        var universityFaker = new Faker<University>();
        var dbUniversity = universityFaker
            .RuleFor(a => a.UniversityId, _autoFaker.Generate<int>())
            .RuleFor(a => a.Name, f => f.Company.CompanyName())
            .Generate();
        
        var academicName = _autoFaker.Generate<string>();
        
        var academicFaker = new Faker<Academic>();
        var dbAcademic = academicFaker
            .RuleFor(u => u.AcademicId, _autoFaker.Generate<int>())
            .RuleFor(u => u.UniversityId, dbUniversity.UniversityId)
            .RuleFor(a => a.Name, academicName)
            .Generate();
        
        _dbContext.Universities.Add(dbUniversity);
        _dbContext.Academics.Add(dbAcademic);
        await _dbContext.SaveChangesAsync();
        
        var command = new AddAcademicCommand(dbUniversity.UniversityId, academicName);
        var handler = new AddAcademicCommandHandler(_dbContext);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Message.Should().Be($"Academic with name \"{academicName}\" already exists.");
    }

    [Fact]
    public async Task Handle_WhenAcademicNotExists_ReturnSuccessResultWithAcademicResponse()
    {
        // Arrange
        var universityFaker = new Faker<University>();
        var dbUniversity = universityFaker
            .RuleFor(a => a.UniversityId, _autoFaker.Generate<int>())
            .RuleFor(a => a.Name, f => f.Company.CompanyName())
            .Generate();
        
        var academicFaker = new Faker<Academic>();
        var dbAcademic = academicFaker
            .RuleFor(u => u.UniversityId, dbUniversity.UniversityId)
            .Generate();
        
        _dbContext.Universities.Add(dbUniversity);
        await _dbContext.SaveChangesAsync();

        var academicName = _autoFaker.Generate<string>();
        
        var academicResponse = new AcademicResponse(
            1,
            dbAcademic.UniversityId,
            academicName,
            []);
        
        var command = new AddAcademicCommand(dbUniversity.UniversityId, academicName);
        var handler = new AddAcademicCommandHandler(_dbContext);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(academicResponse);
    }
}
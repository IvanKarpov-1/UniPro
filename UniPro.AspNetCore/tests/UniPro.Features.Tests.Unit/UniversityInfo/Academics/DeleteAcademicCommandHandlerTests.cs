using UniPro.Domain.Entities;
using UniPro.Features.Features.UniversityInfo.Academics;
using UniPro.Infrastructure.Database;
using Task = System.Threading.Tasks.Task;

namespace UniPro.Features.Tests.Unit.UniversityInfo.Academics;

public class DeleteAcademicCommandHandlerTests
{
    private readonly UniProDbContext _dbContext = Create.MockedDbContextFor<UniProDbContext>();
    private readonly AutoFaker _autoFaker = new();

    [Fact]
    public async Task Handle_WhenNoAcademicInDb_ReturnFailResultWithNotFoundError()
    {
        // Arrange
        var academicId = _autoFaker.Generate<int>();
        var command = new DeleteAcademicCommand(academicId);
        var handler = new DeleteAcademicCommandHandler(_dbContext);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Message.Should().Be($"Academic with ID {academicId} not found.");
    }

    [Fact]
    public async Task Handle_WhenNoAcademicWithProvidedId_ReturnFailResultWithNotFoundError()
    {
        // Arrange
        var faker = new Faker<Academic>();
        var dbAcademic = faker
            .RuleFor(a => a.AcademicId, _autoFaker.Generate<int>())
            .RuleFor(a => a.Name, f => f.Company.CompanyName())
            .Generate();
        
        _dbContext.Academics.Add(dbAcademic);
        await _dbContext.SaveChangesAsync();
        
        var academicId = _autoFaker.Generate<int>();
        var command = new DeleteAcademicCommand(academicId);
        var handler = new DeleteAcademicCommandHandler(_dbContext);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Message.Should().Be($"Academic with ID {academicId} not found.");
    }

    [Fact]
    public async Task Handle_WhenAcademicWithProvidedId_ReturnSuccessResult()
    {
        // Arrange
        var faker = new Faker<Academic>();
        var dbAcademic = faker
            .RuleFor(a => a.AcademicId, _autoFaker.Generate<int>())
            .RuleFor(a => a.Name, f => f.Company.CompanyName())
            .Generate();
        
        _dbContext.Academics.Add(dbAcademic);
        await _dbContext.SaveChangesAsync();

        var academicId = dbAcademic.AcademicId;
        var command = new DeleteAcademicCommand(academicId);
        var handler = new DeleteAcademicCommandHandler(_dbContext);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        var academic = await _dbContext.Academics.FindAsync(academicId);
        
        result.IsSuccess.Should().BeTrue();
        academic.Should().BeNull();
    }
}
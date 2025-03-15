using UniPro.Domain.Entities;
using UniPro.Features.Features.UniversityInfo.Universities;
using UniPro.Infrastructure.Database;
using Task = System.Threading.Tasks.Task;

namespace UniPro.Features.Tests.Unit.UniversityInfo.Universities;

public class DeleteUniversityCommandHandlerTests
{
    private readonly UniProDbContext _dbContext = Create.MockedDbContextFor<UniProDbContext>();
    private readonly AutoFaker _autoFaker = new();

    [Fact]
    public async Task Handle_WhenNoUniversityInDb_ReturnFailResultWithNotFoundError()
    {
        // Arrange
        var universityId = _autoFaker.Generate<int>();
        var command = new DeleteUniversityCommand(universityId);
        var handler = new DeleteUniversityCommandHandler(_dbContext);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Message.Should().Be($"University with ID {universityId} not found.");
    }

    [Fact]
    public async Task Handle_WhenNoUniversityWithProvidedId_ReturnFailResultWithNotFoundError()
    {
        // Arrange
        var faker = new Faker<University>();
        var dbUniversity = faker
            .RuleFor(a => a.Name, f => f.Company.CompanyName())
            .Generate();
        
        _dbContext.Universities.Add(dbUniversity);
        await _dbContext.SaveChangesAsync();
        
        var universityId = _autoFaker.Generate<int>();
        var command = new DeleteUniversityCommand(universityId);
        var handler = new DeleteUniversityCommandHandler(_dbContext);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Message.Should().Be($"University with ID {universityId} not found.");
    }

    [Fact]
    public async Task Handle_WhenUniversityWithProvidedId_ReturnSuccessResult()
    {
        // Arrange
        var faker = new Faker<University>();
        var dbUniversity = faker
            .RuleFor(a => a.Name, f => f.Company.CompanyName())
            .Generate();
        
        _dbContext.Universities.Add(dbUniversity);
        await _dbContext.SaveChangesAsync();

        var universityId = dbUniversity.UniversityId;
        var command = new DeleteUniversityCommand(universityId);
        var handler = new DeleteUniversityCommandHandler(_dbContext);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        var university = await _dbContext.Universities.FindAsync(universityId);
        
        result.IsSuccess.Should().BeTrue();
        university.Should().BeNull();
    }
}
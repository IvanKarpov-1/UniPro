using UniPro.Domain.Entities;
using UniPro.Features.Features.UniversityInfo.Universities;
using UniPro.Infrastructure.Database;
using Task = System.Threading.Tasks.Task;

namespace UniPro.Features.Tests.Unit.UniversityInfo.Universities;

public class UpdateUniversityCommandHandlerTests
{
    private readonly UniProDbContext _dbContext = Create.MockedDbContextFor<UniProDbContext>();
    private readonly AutoFaker _autoFaker = new();
    
    [Fact]
    public async Task Handle_WhenNoUniversityInDb_ReturnFailResultWithNotFoundError()
    {
        // Arrange
        var universityId = _autoFaker.Generate<int>();
        var command = new UpdateUniversityCommand(
            universityId,
            null!);
        var handler = new UpdateUniversityCommandHandler(_dbContext);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Message.Should().Be($"University with ID {universityId} not found.");
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("University name")]
    public async Task Handle_WhenUniversityWithProvidedId_ReturnSuccessResult(string newUniversityName)
    {
        // Arrange
        var faker = new Faker<University>();
        var dbUniversity = faker
            .RuleFor(a => a.Name, f => f.Company.CompanyName())
            .Generate();
        _dbContext.Universities.Add(dbUniversity);
        await _dbContext.SaveChangesAsync();
        
        var command = new UpdateUniversityCommand(
            dbUniversity.UniversityId,
            newUniversityName);

        var handler = new UpdateUniversityCommandHandler(_dbContext);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        var updatedUniversity = await _dbContext.Universities.FindAsync(dbUniversity.UniversityId);
        
        result.IsSuccess.Should().BeTrue();
        updatedUniversity!.Name.Should().Be(newUniversityName);
    }
}
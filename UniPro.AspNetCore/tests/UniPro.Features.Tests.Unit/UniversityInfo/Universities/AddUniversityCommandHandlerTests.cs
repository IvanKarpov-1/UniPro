using UniPro.Domain.Entities;
using UniPro.Features.Common.Responses;
using UniPro.Features.Features.UniversityInfo.Universities;
using UniPro.Infrastructure.Database;
using Task = System.Threading.Tasks.Task;

namespace UniPro.Features.Tests.Unit.UniversityInfo.Universities;

public class AddUniversityCommandHandlerTests
{
    private readonly UniProDbContext _dbContext = Create.MockedDbContextFor<UniProDbContext>();
    private readonly AutoFaker _autoFaker = new();

    [Fact]
    public async Task Handle_WhenUniversityExists_ReturnFailResultWithAlreadyExistError()
    {
        // Arrange
        var universityName = _autoFaker.Generate<string>();
        
        var universityFaker = new Faker<University>();
        var dbUniversity = universityFaker
            .RuleFor(a => a.UniversityId, _autoFaker.Generate<int>())
            .RuleFor(a => a.Name, universityName)
            .Generate();
        
        _dbContext.Universities.Add(dbUniversity);
        await _dbContext.SaveChangesAsync();
        
        var command = new AddUniversityCommand(universityName);
        var handler = new AddUniversityCommandHandler(_dbContext);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Message.Should().Be($"University with name \"{universityName}\" already exists.");
    }

    [Fact]
    public async Task Handle_WhenUniversityNotExists_ReturnSuccessResultWithUniversityResponse()
    {
        // Arrange
        var universityName = _autoFaker.Generate<string>();
        
        var universityResponse = new UniversityResponse(
            1,
            universityName,
            []);
        
        var command = new AddUniversityCommand(universityName);
        var handler = new AddUniversityCommandHandler(_dbContext);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(universityResponse);
    }
}
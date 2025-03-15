using UniPro.Domain.Entities;
using UniPro.Features.Common.Responses;
using UniPro.Features.Features.UniversityInfo.Universities;
using UniPro.Infrastructure.Database;
using Task = System.Threading.Tasks.Task;

namespace UniPro.Features.Tests.Unit.UniversityInfo.Universities;

public class GetUniversityQueryHandlerTests
{
    private readonly UniProDbContext _dbContext = Create.MockedDbContextFor<UniProDbContext>();
    private readonly AutoFaker _autoFaker = new();

    [Fact]
    public async Task Handle_WhenNoUniversityInDb_ReturnFailResultWithNotFoundError()
    {
        // Arrange
        var universityId = _autoFaker.Generate<int>();
        var query = new GetUniversityQuery(universityId);
        var handler = new GetUniversityQueryHandler(_dbContext);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

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
        var query = new GetUniversityQuery(universityId);
        var handler = new GetUniversityQueryHandler(_dbContext);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Message.Should().Be($"University with ID {universityId} not found.");
    }

    [Fact]
    public async Task Handle_WhenUniversityWithProvidedId_ReturnSuccessResultWithUniversityResponse()
    {
        // Arrange
        var faker = new Faker<University>();
        var dbUniversity = faker
            .RuleFor(a => a.UniversityId, _autoFaker.Generate<int>())
            .RuleFor(a => a.UniversityId, _autoFaker.Generate<int>())
            .RuleFor(a => a.Name, f => f.Company.CompanyName())
            .RuleFor(a => a.Academics, new List<Academic>())
            .Generate();
        _dbContext.Universities.Add(dbUniversity);
        await _dbContext.SaveChangesAsync();

        var universityId = dbUniversity.UniversityId;
        var query = new GetUniversityQuery(universityId);
        var handler = new GetUniversityQueryHandler(_dbContext);

        var universityResponse = new UniversityResponse(
            universityId,
            dbUniversity.Name,
            []);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(universityResponse);
    }
}
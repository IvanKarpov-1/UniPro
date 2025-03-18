using Microsoft.EntityFrameworkCore;
using UniPro.Features.Common.Responses;
using UniPro.Features.Tests.Integration.Setup;
using UniPro.Infrastructure.Database;

namespace UniPro.Features.Tests.Integration.UniversityInfo.Universities;

[Collection(nameof(UniProWebApplicationFactory))]
public class GetAllUniversitiesTests(UniProWebApplicationFactory factory) : IAsyncLifetime
{
    private readonly HttpClient _client = factory.HttpClient;
    private readonly UniProDbContext _dbContext = factory.DbContext;
    private readonly Func<Task> _resetDatabase = factory.ResetDatabaseAsync;

    [Fact]
    public async Task Get_WhenUniversitiesUsersInDb_ReturnsEmptyCollection()
    {
        // Clearing the DB, as Respawner can't clear universities table
        await _dbContext.CleanDbAsync();

        // Arrange

        // Act
        var response = await _client.GetAsync("/api/universities");
        var universityResponses = await response.Content.ReadFromJsonAsync<List<UniversityResponse>>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        universityResponses.Should().NotBeNull();
        universityResponses.Count.Should().Be(0);
    }

    [Fact]
    public async Task Get_WhenUniversitiesInDb_ReturnsUniversities()
    {
        // Clearing the DB, as Respawner can't clear universities table
        await _dbContext.CleanDbAsync();

        // Arrange
        var populateUniversityInfoResult = await _dbContext.PopulateUniversityInfoAsync();

        var universityResponses = new List<UniversityResponse>()
        {
            new(populateUniversityInfoResult.University.UniversityId,
                populateUniversityInfoResult.University.Name,
                [])
        };

        // Act
        var response = await _client.GetAsync("/api/universities");
        var universities = await response.Content.ReadFromJsonAsync<List<UniversityResponse>>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        universities.Should().NotBeNull();
        universities.Count.Should().Be(1);
        universities.Should().BeEquivalentTo(universityResponses);
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        await _resetDatabase();
        _client.RemoveTestHeaders();
    }
}
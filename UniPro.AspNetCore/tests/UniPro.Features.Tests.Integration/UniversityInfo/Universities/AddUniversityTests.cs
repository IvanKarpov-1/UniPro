using UniPro.Features.Common.Responses;
using UniPro.Features.Features.UniversityInfo.Universities;
using UniPro.Features.Tests.Integration.Setup;
using UniPro.Infrastructure.Database;

namespace UniPro.Features.Tests.Integration.UniversityInfo.Universities;

[Collection(nameof(UniProWebApplicationFactory))]
public class AddUniversityTests(UniProWebApplicationFactory factory) : IAsyncLifetime
{
    private readonly HttpClient _client = factory.HttpClient;
    private readonly UniProDbContext _dbContext = factory.DbContext;
    private readonly Func<Task> _resetDatabase = factory.ResetDatabaseAsync;
    private readonly AutoFaker _autoFaker = new();

    [Fact]
    public async Task Post_WhenUserNotAuthenticated_ReturnUnauthorizedProblem()
    {
        // Arrange
        var addUniversityRequest = new AddUniversityRequest("");
        _client.OmitAuthentication();
        
        // Act
        var response = await _client.PostAsJsonAsync("/api/universities", addUniversityRequest);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Post_WhenUniversityAlreadyExists_ReturnConflictProblem()
    {
        // Arrange
        var populateUniversityInfoResult = await _dbContext.PopulateUniversityInfoAsync();
        var universityName = populateUniversityInfoResult.University.Name;
        var addUniversityRequest = new AddUniversityRequest(universityName);

        // Act
        var response = await _client.PostAsJsonAsync("/api/universities", addUniversityRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Theory]
    [InlineData("University Name")]
    public async Task Post_WhenUniversityNotExists_ReturnCreated(string universityName)
    {
        // Arrange
        var addUniversityRequest = new AddUniversityRequest(universityName);

        var universityResponse = new UniversityResponse(
            1,
            universityName,
            []);
        
        // Act
        var response = await _client.PostAsJsonAsync("/api/universities", addUniversityRequest);
        var university = await response.Content.ReadFromJsonAsync<UniversityResponse>();
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        university.Should().NotBeNull();
        response.Headers.Location.Should().NotBeNull();
        response.Headers.Location.AbsolutePath.Should().Be($"/api/universities/{university.UniversityId}");
        university.Should().BeEquivalentTo(universityResponse);
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync() {
        await _resetDatabase();
        _client.RemoveTestHeaders();
    }
}
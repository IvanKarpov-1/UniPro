using UniPro.Features.Tests.Integration.Setup;
using UniPro.Infrastructure.Database;

namespace UniPro.Features.Tests.Integration.UniversityInfo.Universities;

[Collection(nameof(UniProWebApplicationFactory))]
public class DeleteUniversityTests(UniProWebApplicationFactory factory) : IAsyncLifetime
{
    private readonly HttpClient _client = factory.HttpClient;
    private readonly UniProDbContext _dbContext = factory.DbContext;
    private readonly Func<Task> _resetDatabase = factory.ResetDatabaseAsync;
    private readonly AutoFaker _autoFaker = new();

    [Fact]
    public async Task Post_WhenUserNotAuthenticated_ReturnUnauthorizedProblem()
    {
        // Arrange
        var universityId = _autoFaker.Generate<int>();
        _client.OmitAuthentication();
        
        // Act
        var response = await _client.DeleteAsync($"/api/universities/{universityId}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Post_WhenNoUniversityWithProvidedId_ReturnNotFoundProblem()
    {
        // Arrange
        var universityId = _autoFaker.Generate<int>();
        
        // Act
        var response = await _client.DeleteAsync($"/api/universities/{universityId}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Post_WhenUniversityWithProvidedId_ReturnNoContent()
    {
        // Arrange
        var populateUniversityInfoResult = await _dbContext.PopulateUniversityInfoAsync();
        var dbUniversity = populateUniversityInfoResult.University;
        
        // Act
        var response = await _client.DeleteAsync($"/api/universities/{dbUniversity.UniversityId}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        var university = await _dbContext.Universities.FindAsync(dbUniversity.UniversityId);
        university.Should().BeNull();
    }
    
    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync() {
        await _resetDatabase();
        _client.RemoveTestHeaders();
    }
}
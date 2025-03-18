using UniPro.Features.Features.UniversityInfo.Universities;
using UniPro.Features.Tests.Integration.Setup;
using UniPro.Infrastructure.Database;

namespace UniPro.Features.Tests.Integration.UniversityInfo.Universities;

[Collection(nameof(UniProWebApplicationFactory))]
public class UpdateUniversityTests(UniProWebApplicationFactory factory) : IAsyncLifetime
{
    private readonly HttpClient _client = factory.HttpClient;
    private readonly UniProDbContext _dbContext = factory.DbContext;
    private readonly Func<Task> _resetDatabase = factory.ResetDatabaseAsync;
    private readonly AutoFaker _autoFaker = new();
    
    [Fact]
    public async Task Put_WhenUserNotAuthenticated_ReturnUnauthorizedProblem()
    {
        // Arrange
        var universityId = _autoFaker.Generate<int>();
        var updateUniversityRequest = new UpdateUniversityRequest("");
        _client.OmitAuthentication();
        
        // Act
        var response = await _client.PutAsJsonAsync($"/api/universities/{universityId}", updateUniversityRequest);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Put_WhenNoUniversityWithProvidedId_ReturnNotFoundProblem()
    {
        // Arrange
        await _dbContext.PopulateUniversityInfoAsync();
        var universityId = _autoFaker.Generate<int>();
        var updateUniversityRequest = new UpdateUniversityRequest("");
        
        // Act
        var response = await _client.PutAsJsonAsync($"/api/universities/{universityId}", updateUniversityRequest);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Theory]
    [InlineData("New University Name")]
    public async Task Put_WhenUniversityWithProvidedId_ReturnNoContent(string newUniversityName)
    {
        // Arrange
        var populateUniversityInfoResult = await _dbContext.PopulateUniversityInfoAsync();
        var universityId = populateUniversityInfoResult.University.UniversityId;

        var updateUniversityRequest = new UpdateUniversityRequest(newUniversityName);
        
        // Act
        var response = await _client.PutAsJsonAsync($"/api/universities/{universityId}", updateUniversityRequest);
        
        // Assert
        var updatedUniversity = await _dbContext.Universities.FindAsync(universityId);
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        updatedUniversity!.Name.Should().Be(newUniversityName);
    }
    
    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()  {
        await _resetDatabase();
        _client.RemoveTestHeaders();
    }
}
using UniPro.Features.Common.Responses;
using UniPro.Features.Tests.Integration.Setup;
using UniPro.Infrastructure.Database;
using Task = System.Threading.Tasks.Task;

namespace UniPro.Features.Tests.Integration.UniversityInfo.Universities;

[Collection(nameof(UniProWebApplicationFactory))]
public class GetUniversityTests(UniProWebApplicationFactory factory) : IAsyncLifetime
{
    private readonly HttpClient _client = factory.HttpClient;
    private readonly UniProDbContext _dbContext = factory.DbContext;
    private readonly Func<Task> _resetDatabase = factory.ResetDatabaseAsync;
    private readonly AutoFaker _autoFaker = new();

    [Fact]
    public async Task Get_WhenNoUniversitiesInDb_ReturnsNotFoundProblem()
    {
        // Arrange
        var universityId = _autoFaker.Generate<int>();

        // Act
        var response = await _client.GetAsync($"/api/universities/{universityId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Get_WhenUniversitiesInDbButWithoutUniversityWithProvidedId_ReturnsNotFoundProblem()
    {
        // Arrange
        await _dbContext.PopulateUniversityInfoAsync();
        var universityId = _autoFaker.Generate<int>();

        // Act
        var response = await _client.GetAsync($"/api/universities/{universityId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Get_WhenUniversityWithProvidedId_ReturnsOk()
    {
        // Arrange
        var populateUniversityInfoResult = await _dbContext.PopulateUniversityInfoAsync();
        var universityId = populateUniversityInfoResult.University.UniversityId;

        var universityResponse = new UniversityResponse(
            populateUniversityInfoResult.University.UniversityId,
            populateUniversityInfoResult.University.Name,
            [
                new AcademicResponse(
                    populateUniversityInfoResult.Academic.AcademicId,
                    populateUniversityInfoResult.University.UniversityId,
                    populateUniversityInfoResult.Academic.Name,
                        [])
            ]);
        
        // Act
        var response = await _client.GetAsync($"/api/universities/{universityId}");
        var university = await response.Content.ReadFromJsonAsync<UniversityResponse>();
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        university.Should().NotBeNull();
        university.Should().BeEquivalentTo(universityResponse);
    }
    
    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync() => await _resetDatabase();
}
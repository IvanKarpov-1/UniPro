using UniPro.Features.Common.Responses;
using UniPro.Features.Tests.Integration.Setup;
using UniPro.Infrastructure.Database;
using Task = System.Threading.Tasks.Task;

namespace UniPro.Features.Tests.Integration.Users;

[Collection(nameof(UniProWebApplicationFactory))]
public class GetAllUsersTests(UniProWebApplicationFactory factory) : IAsyncLifetime
{
    private readonly HttpClient _client = factory.HttpClient;
    private readonly UniProDbContext _dbContext = factory.DbContext;
    private readonly Func<Task> _resetDatabase = factory.ResetDatabaseAsync;

    [Fact]
    public async Task Get_WhenNoUsersInDb_ReturnsEmptyCollection()
    {
        // Arrange

        // Act
        var response = await _client.GetAsync("/api/users");
        var users = await response.Content.ReadFromJsonAsync<List<UserResponse>>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        users.Should().NotBeNull();
        users.Count.Should().Be(0);
    }

    [Fact]
    public async Task Get_WhenUsersInDb_ReturnsUsers()
    {
        // Arrange
        const int usersCount = 2;

        var dbUsers = await _dbContext.PopulateUsers(usersCount);
        
        var userResponses = dbUsers.Select(u => new UserResponse
        (
            u.UserId,
            u.FirstName,
            u.LastName,
            u.Patronymic,
            u.Avatar!,
            u.PhoneNumber
        ));

        // Act
        var response = await _client.GetAsync("/api/users");
        var users = await response.Content.ReadFromJsonAsync<List<UserResponse>>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        users.Should().NotBeNull();
        users.Count.Should().Be(usersCount);
        users.Should().BeEquivalentTo(userResponses);
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync() => _resetDatabase();
}
using System.Security.Claims;
using UniPro.Features.Features.Users;
using UniPro.Features.Tests.Integration.Setup;
using UniPro.Infrastructure.Database;

namespace UniPro.Features.Tests.Integration.Users;

[Collection(nameof(UniProWebApplicationFactory))]
public class UpdateUserTests(UniProWebApplicationFactory factory) : IAsyncLifetime
{
    private readonly HttpClient _client = factory.HttpClient;
    private readonly UniProDbContext _dbContext = factory.DbContext;
    private readonly Func<Task> _resetDatabase = factory.ResetDatabaseAsync;
    private readonly AutoFaker _autoFaker = new();

    [Fact]
    public async Task Put_WhenUserNotAuthenticated_ReturnUnauthorizedProblem()
    {
        // Arrange
        var userId = _autoFaker.Generate<Guid>().ToString();
        var updateUserRequest = new UpdateUserRequest(null, null);
        _client.OmitAuthentication();
        
        // Act
        var response = await _client.PutAsJsonAsync($"/api/users/{userId}", updateUserRequest);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Put_WhenUserIsAuthenticatedButNotAuthorized_ReturnForbiddenProblem()
    {
        // Arrange
        var userId = _autoFaker.Generate<Guid>().ToString();
        var userId2 = _autoFaker.Generate<Guid>().ToString();
        var updateUserRequest = new UpdateUserRequest(null, null);
        _client.AddClaims([new Claim(ClaimTypes.NameIdentifier, userId2)]);
        
        // Act
        var response = await _client.PutAsJsonAsync($"/api/users/{userId}", updateUserRequest);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Put_WhenNoUserWithProvidedId_ReturnNotFoundProblem()
    {
        // Arrange
        await _dbContext.PopulateUsersAsync(1);
        var userId = _autoFaker.Generate<Guid>().ToString();
        _client.AddClaims([new Claim(ClaimTypes.NameIdentifier, userId)]);

        var updateUserRequest = new UpdateUserRequest(null, null);
        
        // Act
        var response = await _client.PutAsJsonAsync($"/api/users/{userId}", updateUserRequest);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Put_WhenUserWithProvidedIdButEmptyRequestValues_ReturnBadRequestProblem()
    {
        // Arrange
        var dbUser = (await _dbContext.PopulateUsersAsync(1))[0];
        var userId = dbUser.UserId;
        _client.AddClaims([new Claim(ClaimTypes.NameIdentifier, userId)]);

        var updateUserRequest = new UpdateUserRequest(null, null);
        
        // Act
        var response = await _client.PutAsJsonAsync($"/api/users/{userId}", updateUserRequest);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData("avatar", null)]
    [InlineData(null, "phoneNumber")]
    [InlineData("avatar", "phoneNumber")]
    public async Task Put_WhenUserWithProvidedIdAndCorrectRequestValues_ReturnNoContent(string? avatar, string? phoneNumber)
    {
        // Arrange
        var dbUser = (await _dbContext.PopulateUsersAsync(1))[0];
        var userId = dbUser.UserId;
        _client.AddClaims([new Claim(ClaimTypes.NameIdentifier, userId)]);

        var updateUserRequest = new UpdateUserRequest(avatar, phoneNumber);
        
        // Act
        var response = await _client.PutAsJsonAsync($"/api/users/{userId}", updateUserRequest);
        
        // Assert
        var updatedUser = await _dbContext.Users.FindAsync(dbUser.UserId);
        
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        updatedUser!.Avatar.Should().Be(avatar ?? dbUser.Avatar);
        updatedUser!.PhoneNumber.Should().Be(phoneNumber ?? dbUser.PhoneNumber);
    }

    public Task InitializeAsync() => Task.CompletedTask;
    
    public async Task DisposeAsync() {
        await _resetDatabase();
        _client.RemoveTestHeaders();
    }
}
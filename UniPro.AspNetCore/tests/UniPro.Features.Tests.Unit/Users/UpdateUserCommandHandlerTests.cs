using UniPro.Domain.Entities;
using UniPro.Infrastructure.Database;
using UniPro.Features.Features.Users;
using Task = System.Threading.Tasks.Task;

namespace UniPro.Features.Tests.Unit.Users;

public class UpdateUserCommandHandlerTests
{
    private readonly UniProDbContext _dbContext = Create.MockedDbContextFor<UniProDbContext>();
    private readonly AutoFaker _autoFaker = new();

    [Fact]
    public async Task Handle_WhenNoUserInDb_ReturnFailResultWithNotFoundError()
    {
        // Arrange
        var userId = _autoFaker.Generate<Guid>().ToString();
        var command = new UpdateUserCommand(null, null)
        {
            UserId = userId
        };
        var handler = new UpdateUserCommandHandler(_dbContext);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Message.Should().Be($"User with ID {userId} not found.");
    }

    [Fact]
    public async Task Handle_WhenUserWithProvidedIdButWithoutProperRequest_ReturnFailResultWithBadRequestError()
    {
        // Arrange
        var faker = new Faker<User>();
        var dbUser = faker
            .RuleFor(u => u.UserId, _autoFaker.Generate<Guid>().ToString())
            .RuleFor(u => u.AppId, "public")
            .RuleFor(u => u.FirstName, f => f.Name.FirstName())
            .RuleFor(u => u.LastName, f => f.Name.LastName())
            .RuleFor(u => u.Patronymic, f => f.Name.FullName())
            .RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber())
            .RuleFor(u => u.Avatar, f => f.Internet.Avatar())
            .Generate();
        _dbContext.Users.Add(dbUser);
        await _dbContext.SaveChangesAsync();
        
        var command = new UpdateUserCommand(null, null)
        {
            UserId = dbUser.UserId
        };
        var handler = new UpdateUserCommandHandler(_dbContext);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Message.Should().Be($"Please provide a valid avatar or phone number.");
    }

    [Theory]
    [InlineData("avatar", null)]
    [InlineData(null, "phoneNumber")]
    [InlineData("avatar", "phoneNumber")]
    public async Task Handle_WhenUserWithProvidedId_ReturnSuccessResult(string? avatar, string? phoneNumber)
    {
        // Arrange
        var faker = new Faker<User>();
        var dbUser = faker
            .RuleFor(u => u.UserId, _autoFaker.Generate<Guid>().ToString())
            .RuleFor(u => u.AppId, "public")
            .RuleFor(u => u.FirstName, f => f.Name.FirstName())
            .RuleFor(u => u.LastName, f => f.Name.LastName())
            .RuleFor(u => u.Patronymic, f => f.Name.FullName())
            .RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber())
            .RuleFor(u => u.Avatar, f => f.Internet.Avatar())
            .Generate();
        _dbContext.Users.Add(dbUser);
        await _dbContext.SaveChangesAsync();
        _dbContext.ChangeTracker.Clear();
        
        var command = new UpdateUserCommand(avatar, phoneNumber)
        {
            UserId = dbUser.UserId
        };
        var handler = new UpdateUserCommandHandler(_dbContext);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        var updatedUser = await _dbContext.Users.FindAsync(dbUser.UserId);
        
        result.IsSuccess.Should().BeTrue();
        updatedUser!.Avatar.Should().Be(avatar ?? dbUser.Avatar);
        updatedUser!.PhoneNumber.Should().Be(phoneNumber ?? dbUser.PhoneNumber);
    }
}
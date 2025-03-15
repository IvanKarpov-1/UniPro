using UniPro.Domain.Entities;
using UniPro.Features.Features.UniversityInfo.Academics;
using UniPro.Infrastructure.Database;
using Task = System.Threading.Tasks.Task;

namespace UniPro.Features.Tests.Unit.UniversityInfo.Academics;

public class UpdateAcademicCommandHandlerTests
{
    private readonly UniProDbContext _dbContext = Create.MockedDbContextFor<UniProDbContext>();
    private readonly AutoFaker _autoFaker = new();
    
    [Fact]
    public async Task Handle_WhenNoAcademicInDb_ReturnFailResultWithNotFoundError()
    {
        // Arrange
        var academicId = _autoFaker.Generate<int>();
        var command = new UpdateAcademicCommand(
            academicId,
            _autoFaker.Generate<string>(),
            null);
        var handler = new UpdateAcademicCommandHandler(_dbContext);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Message.Should().Be($"Academic with ID {academicId} not found.");
    }
    
    [Theory]
    [InlineData("Academic name", null)]
    [InlineData("Academic name", 2)]
    public async Task Handle_WhenAcademicWithProvidedId_ReturnSuccessResult(string newAcademicName, int? newUniversityId)
    {
        // Arrange
        var faker = new Faker<Academic>();
        var dbAcademic = faker
            .RuleFor(a => a.AcademicId, _autoFaker.Generate<int>())
            .RuleFor(a => a.UniversityId, _autoFaker.Generate<int>())
            .RuleFor(a => a.Name, f => f.Company.CompanyName())
            .Generate();
        _dbContext.Academics.Add(dbAcademic);
        await _dbContext.SaveChangesAsync();
        
        var command = new UpdateAcademicCommand(dbAcademic.AcademicId,
            newAcademicName,
            newUniversityId);

        var handler = new UpdateAcademicCommandHandler(_dbContext);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        var updatedAcademic = await _dbContext.Academics.FindAsync(dbAcademic.AcademicId);
        
        result.IsSuccess.Should().BeTrue();
        updatedAcademic!.Name.Should().Be(newAcademicName);
        updatedAcademic!.UniversityId.Should().Be(newUniversityId ?? dbAcademic.UniversityId);
    }
}
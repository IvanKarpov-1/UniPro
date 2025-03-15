using UniPro.Domain.Entities;
using UniPro.Features.Common.Responses;
using UniPro.Features.Features.UniversityInfo.Academics;
using UniPro.Infrastructure.Database;
using Task = System.Threading.Tasks.Task;

namespace UniPro.Features.Tests.Unit.UniversityInfo.Academics;

public class GetAcademicQueryHandlerTests
{
    private readonly UniProDbContext _dbContext = Create.MockedDbContextFor<UniProDbContext>();
    private readonly AutoFaker _autoFaker = new();

    [Fact]
    public async Task Handle_WhenNoAcademicInDb_ReturnFailResultWithNotFoundError()
    {
        // Arrange
        var academicId = _autoFaker.Generate<int>();
        var query = new GetAcademicQuery(academicId);
        var handler = new GetAcademicQueryHandler(_dbContext);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Message.Should().Be($"Academic with ID {academicId} not found.");
    }

    [Fact]
    public async Task Handle_WhenNoAcademicWithProvidedId_ReturnFailResultWithNotFoundError()
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
        
        var academicId = _autoFaker.Generate<int>();
        var query = new GetAcademicQuery(academicId);
        var handler = new GetAcademicQueryHandler(_dbContext);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Message.Should().Be($"Academic with ID {academicId} not found.");
    }

    [Fact]
    public async Task Handle_WhenAcademicWithProvidedId_ReturnSuccessResultWithAcademicResponse()
    {
        // Arrange
        var faker = new Faker<Academic>();
        var dbAcademic = faker
            .RuleFor(a => a.AcademicId, _autoFaker.Generate<int>())
            .RuleFor(a => a.UniversityId, _autoFaker.Generate<int>())
            .RuleFor(a => a.Name, f => f.Company.CompanyName())
            .RuleFor(a => a.Departments, new List<Department>())
            .Generate();
        _dbContext.Academics.Add(dbAcademic);
        await _dbContext.SaveChangesAsync();

        var academicId = dbAcademic.AcademicId;
        var query = new GetAcademicQuery(academicId);
        var handler = new GetAcademicQueryHandler(_dbContext);

        var academicResponse = new AcademicResponse(
            academicId,
            dbAcademic.UniversityId,
            dbAcademic.Name,
            []);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(academicResponse);
    }
}
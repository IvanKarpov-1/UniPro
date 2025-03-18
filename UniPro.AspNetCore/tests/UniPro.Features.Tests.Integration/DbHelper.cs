using UniPro.Domain.Entities;
using UniPro.Domain.Entities.SuperTokens;
using UniPro.Infrastructure.Database;

namespace UniPro.Features.Tests.Integration;

public static class DbHelper
{
    public record PopulateUniversityInfoResult(
        University University,
        Academic Academic,
        Department Department,
        StudentGroup StudentGroup);

    private static readonly AutoFaker AutoFaker = new();

    public static async Task<List<User>> PopulateUsers(this UniProDbContext dbContext, int usersCount = 2)
    {
        var userIds = new List<string>();

        for (var i = 0; i < usersCount; i++)
        {
            userIds.Add(AutoFaker.Generate<Guid>().ToString());
        }

        var stAppIdToUserIdFaker = new Faker<StAppIdToUserId>()
            .RuleFor(s => s.AppId, "public")
            .RuleFor(s => s.UserId, f => userIds[f.IndexFaker % usersCount])
            .RuleFor(s => s.RecipeId, f => userIds[f.IndexFaker % usersCount])
            .RuleFor(s => s.PrimaryOrRecipeUserId, f => userIds[f.IndexFaker % usersCount])
            .Generate(usersCount);
        dbContext.StAppIdToUserIds.AddRange(stAppIdToUserIdFaker);

        var faker = new Faker<User>();
        var dbUsers = faker
            .RuleFor(u => u.AppId, "public")
            .RuleFor(u => u.UserId, f => userIds[f.IndexFaker % usersCount])
            .RuleFor(u => u.FirstName, f => f.Name.FirstName())
            .RuleFor(u => u.LastName, f => f.Name.LastName())
            .RuleFor(u => u.Patronymic, f => f.Name.FullName())
            .RuleFor(u => u.Avatar, f => f.Internet.Avatar())
            .RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber())
            .Generate(usersCount);

        dbContext.Users.AddRange(dbUsers);

        await dbContext.SaveChangesAsync();
        dbContext.ChangeTracker.Clear();

        return dbUsers;
    }

    public static async Task<PopulateUniversityInfoResult> PopulateUniversityInfo(this UniProDbContext dbContext)
    {
        var university = new Faker<University>()
            .RuleFor(u => u.UniversityId, AutoFaker.Generate<int>())
            .RuleFor(u => u.Name, f => f.Company.CompanyName())
            .Generate();
        var academic = new Faker<Academic>()
            .RuleFor(a => a.AcademicId, AutoFaker.Generate<int>())
            .RuleFor(a => a.Name, f => f.Company.CompanyName())
            .RuleFor(a => a.UniversityId, university.UniversityId)
            .Generate();
        var department = new Faker<Department>()
            .RuleFor(d => d.DepartmentId, AutoFaker.Generate<int>())
            .RuleFor(d => d.Name, f => f.Company.CompanyName())
            .RuleFor(d => d.AcademicId, academic.AcademicId)
            .Generate();
        var studentGroup = new Faker<StudentGroup>()
            .RuleFor(s => s.StudentGroupId, AutoFaker.Generate<int>())
            .RuleFor(s => s.Name, f => f.Company.CompanyName())
            .RuleFor(s => s.DepartmentId, department.DepartmentId)
            .Generate();

        dbContext.Universities.Add(university);
        dbContext.Academics.Add(academic);
        dbContext.Departments.Add(department);
        dbContext.StudentGroups.Add(studentGroup);

        await dbContext.SaveChangesAsync();
        dbContext.ChangeTracker.Clear();

        return new PopulateUniversityInfoResult(
            university, 
            academic, 
            department,
            studentGroup);
    }
}
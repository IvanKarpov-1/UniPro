using Carter;
using FluentResults;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using UniPro.Features.Common;
using UniPro.Features.Common.Errors;
using UniPro.Features.Extensions;
using UniPro.Infrastructure.Database;

namespace UniPro.Features.Features.Users;

public sealed record UserResponse(
    string UserId,
    string FirstName,
    string LastName,
    string Patronymic,
    string Avatar,
    string PhoneNumber)
{
    public string? UserRole { get; set; }
    
    public string? UniversityName { get; set; }

    public string? AcademicName { get; set; }
    
    public string? DepartmentName { get; set; }
    
    public string? StudentGroupName { get; set; }
}

public sealed record GetUserQuery(
    string UserId)
    : IRequest<Result<UserResponse>>;

public sealed class GetUserQueryHandler(
    UniProDbContext dbContext)
    : IRequestHandler<GetUserQuery, Result<UserResponse>>
{
    public async Task<Result<UserResponse>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users.FindAsync([request.UserId], cancellationToken);

        if (user is null)
        {
            return Result.Fail(new NotFoundError($"User with ID {request.UserId} not found."));
        }

        var userResponse = user.Adapt<UserResponse>();
        
        var userRole = await dbContext
            .StUserRoles
            .FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken);

        userResponse.UserRole = userRole?.Role;
        
        switch (userRole?.Role)
        {
            case "student":
            {
                var student = await dbContext
                    .StudentInfos
                    .Include(x => x.University)
                    .Include(x => x.Academic)
                    .Include(x => x.Department)
                    .Include(x => x.StudentGroup)
                    .FirstOrDefaultAsync(x => x.StudentId == request.UserId, cancellationToken);
            
                student.Adapt(userResponse);
                break;
            }
            case "teacher":
            {
                var student = await dbContext
                    .TeacherInfos
                    .Include(x => x.University)
                    .Include(x => x.Academic)
                    .Include(x => x.Department)
                    .FirstOrDefaultAsync(x => x.TeacherId == request.UserId, cancellationToken);
            
                student.Adapt(userResponse);
                break;
            }
        }

        return Result.Ok(userResponse);
    }
}

public sealed class GetUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/users/{userId}", Handler)
            .RequireAuthorization()
            .Produces<UserResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithName("GetUser")
            .WithTags(EndpointTags.Users);
    }

    private static async Task<IResult> Handler(
        [FromRoute] string userId,
        [FromServices] IHttpContextAccessor httpContextAccessor,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var query = new GetUserQuery(userId);
        var result = await sender.Send(query, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.Errors.ToProblem();
    }
} 
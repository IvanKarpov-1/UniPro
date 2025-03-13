using Carter;
using FluentResults;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using UniPro.Domain.Entities;
using UniPro.Features.Common;
using UniPro.Features.Common.Errors;
using UniPro.Features.Common.Responses;
using UniPro.Features.Extensions;
using UniPro.Infrastructure.Database;
using InternalServerError = UniPro.Features.Common.Errors.InternalServerError;

namespace UniPro.Features.Features.UniversityInfo.StudentGroups;

public sealed record AddStudentGroupRequest(
    int DepartmentId,
    string StudentGroupName);

internal sealed record AddStudentGroupCommand(
    int DepartmentId,
    string StudentGroupName)
    : IRequest<Result<StudentGroupResponse>>;

internal sealed class AddStudentGroupCommandHandler(
    UniProDbContext dbContext) 
    : IRequestHandler<AddStudentGroupCommand, Result<StudentGroupResponse>>
{
    public async Task<Result<StudentGroupResponse>> Handle(
        AddStudentGroupCommand request,
        CancellationToken cancellationToken)
    {
        var department = await dbContext.Departments.FindAsync([request.DepartmentId], cancellationToken);

        if (department is null)
        {
            return Result.Fail(new NotFoundError($"Department with ID {request.DepartmentId} not found."));
        }
        
        var existentStudentGroup = await dbContext.StudentGroups.FirstOrDefaultAsync(
            x => x.Name == request.StudentGroupName, 
            cancellationToken);

        if (existentStudentGroup is not null)
        {
            return Result.Fail(new AlreadyExistError($"Student group with name \"{request.StudentGroupName}\" already exists."));
        }

        var newStudentGroup = new StudentGroup
        {
            Name = request.StudentGroupName,
        };
        
        department.StudentGroups.Add(newStudentGroup);
        
        var result = await dbContext.SaveChangesAsync(cancellationToken);
        
        return result > 0
            ? Result.Ok(newStudentGroup.Adapt<StudentGroupResponse>())
            : Result.Fail(new InternalServerError());
    }
}

public sealed class AddStudentGroupEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/student-groups", Handler)
            .RequireAuthorization()
            .Produces<StudentGroupResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithTags(EndpointTags.StudentGroups);
    }

    private static async Task<IResult> Handler(
        [FromBody] AddStudentGroupRequest request,
        [FromServices] IHttpContextAccessor ctx,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var command = request.Adapt<AddStudentGroupCommand>();
        var result = await sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? Results.CreatedAtRoute(
                "GetStudentGroup",
                new { StudentGroupId = result.Value.StudentGroupId },
                result.Value)
            : result.Errors.ToProblem();
    }
}
using Carter;
using FluentResults;
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

namespace UniPro.Features.Features.UniversityInfo.StudentGroups;

public sealed record UpdateStudentGroupRequest(
    string NewStudentGroupName,
    int? NewDepartmentId);

internal sealed record UpdateStudentGroupCommand(
    int StudentGroupId,
    string NewStudentGroupName,
    int? NewDepartmentId)
    : IRequest<Result<Unit>>;

internal sealed class UpdateStudentGroupCommandHandler(
    UniProDbContext dbContext)
    : IRequestHandler<UpdateStudentGroupCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(UpdateStudentGroupCommand request, CancellationToken cancellationToken)
    {
        var studentGroup = await dbContext.StudentGroups.FirstOrDefaultAsync(
            x => x.StudentGroupId == request.StudentGroupId, 
            cancellationToken);

        if (studentGroup is null)
        {
            return Result.Fail(new NotFoundError($"Student group with ID {request.StudentGroupId} not found."));
        }
        
        studentGroup.Name = request.NewStudentGroupName;

        if (request.NewDepartmentId is not null)
        {
            studentGroup.DepartmentId = (int)request.NewDepartmentId;
        }
        
        var result = await dbContext.SaveChangesAsync(cancellationToken);
        
        return result > 0
            ? Result.Ok()
            : Result.Fail(new InternalServerError());
    }
}

public sealed class UpdateStudentGroupEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/student-groups/{studentGroupId:int}", Handler)
            .RequireAuthorization()
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithTags(EndpointTags.StudentGroups);
    }

    private static async Task<IResult> Handler(
        [FromRoute] int studentGroupId,
        [FromBody] UpdateStudentGroupRequest request,
        [FromServices] IHttpContextAccessor ctx,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new UpdateStudentGroupCommand(studentGroupId, request.NewStudentGroupName, request.NewDepartmentId);
        var result = await sender.Send(command, cancellationToken);

        return result.IsSuccess ? 
            Results.Ok() : 
            result.Errors.ToProblem();
    }
}
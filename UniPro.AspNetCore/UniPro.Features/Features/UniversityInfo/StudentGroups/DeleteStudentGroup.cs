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

internal sealed record DeleteStudentGroupCommand(
    int StudentGroupId)
    : IRequest<Result<Unit>>;

internal sealed class DeleteStudentGroupCommandHandler(
    UniProDbContext dbContext)
    : IRequestHandler<DeleteStudentGroupCommand, Result<Unit>>
{
    public async  Task<Result<Unit>> Handle(DeleteStudentGroupCommand request, CancellationToken cancellationToken)
    {
        var studentGroup = await dbContext.StudentGroups.FirstOrDefaultAsync(
            x => x.StudentGroupId == request.StudentGroupId, 
            cancellationToken);

        if (studentGroup is null)
        {
            return Result.Fail(new NotFoundError($"Student group with ID {request.StudentGroupId} not found."));
        }
        
        dbContext.StudentGroups.Remove(studentGroup);
        
        var result = await dbContext.SaveChangesAsync(cancellationToken);
        
        return result == 0
            ? Result.Ok()
            : Result.Fail(new InternalServerError());
    }
}

public sealed class DeleteStudentGroupEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/student-groups/{studentGroupId:int}", Handler)
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithTags(EndpointTags.StudentGroups);
    }

    private static async Task<IResult> Handler(
        [FromRoute] int studentGroupId,
        [FromServices] HttpContext ctx,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new DeleteStudentGroupCommand(studentGroupId);
        var result = await sender.Send(command, cancellationToken);

        return result.IsSuccess ? 
            Results.StatusCode(StatusCodes.Status204NoContent) : 
            result.Errors.ToProblem();
    }
}
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

namespace UniPro.Features.Features.UniversityInfo.Departments;

internal sealed record DeleteDepartmentCommand(
    int DepartmentId)
    : IRequest<Result<Unit>>;

internal sealed class DeleteDepartmentCommandHandler(
    UniProDbContext dbContext)
    : IRequestHandler<DeleteDepartmentCommand, Result<Unit>>
{
    public async  Task<Result<Unit>> Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
    {
        var department = await dbContext.Departments.FirstOrDefaultAsync(
            x => x.DepartmentId == request.DepartmentId, 
            cancellationToken);

        if (department is null)
        {
            return Result.Fail(new NotFoundError($"Department with ID {request.DepartmentId} not found."));
        }
        
        dbContext.Departments.Remove(department);
        
        var result = await dbContext.SaveChangesAsync(cancellationToken);
        
        return result == 0
            ? Result.Ok()
            : Result.Fail(new InternalServerError());
    }
}

public sealed class DeleteDepartmentEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/departments/{departmentId:int}", Handler)
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithTags(EndpointTags.Departments);
    }

    private static async Task<IResult> Handler(
        [FromRoute] int departmentId,
        [FromServices] HttpContext ctx,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new DeleteDepartmentCommand(departmentId);
        var result = await sender.Send(command, cancellationToken);

        return result.IsSuccess ? 
            Results.StatusCode(StatusCodes.Status204NoContent) : 
            result.Errors.ToProblem();
    }
}
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

public sealed record UpdateDepartmentRequest(
    string NewDepartmentName,
    int? NewAcademicId);

public sealed record UpdateDepartmentCommand(
    int DepartmentId,
    string NewDepartmentName,
    int? NewAcademicId)
    : IRequest<Result<Unit>>;

public sealed class UpdateDepartmentCommandHandler(
    UniProDbContext dbContext)
    : IRequestHandler<UpdateDepartmentCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
    {
        var department = await dbContext.Departments.FirstOrDefaultAsync(
            x => x.DepartmentId == request.DepartmentId, 
            cancellationToken);

        if (department is null)
        {
            return Result.Fail(new NotFoundError($"Department with ID {request.DepartmentId} not found."));
        }
        
        department.Name = request.NewDepartmentName;

        if (request.NewAcademicId is not null)
        {
            department.AcademicId = (int)request.NewAcademicId;
        }
        
        var result = await dbContext.SaveChangesAsync(cancellationToken);
        
        return result > 0
            ? Result.Ok()
            : Result.Fail(new InternalServerError());
    }
}

public sealed class UpdateDepartmentEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/departments/{departmentId:int}", Handler)
            .RequireAuthorization()
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithTags(EndpointTags.Departments);
    }

    private static async Task<IResult> Handler(
        [FromRoute] int departmentId,
        [FromBody] UpdateDepartmentRequest request,
        [FromServices] IHttpContextAccessor ctx,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new UpdateDepartmentCommand(departmentId, request.NewDepartmentName, request.NewAcademicId);
        var result = await sender.Send(command, cancellationToken);

        return result.IsSuccess ? 
            Results.Ok() : 
            result.Errors.ToProblem();
    }
}
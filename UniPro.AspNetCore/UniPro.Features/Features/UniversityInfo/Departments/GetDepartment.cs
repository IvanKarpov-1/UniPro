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
using UniPro.Features.Common.Responses;
using UniPro.Features.Extensions;
using UniPro.Infrastructure.Database;

namespace UniPro.Features.Features.UniversityInfo.Departments;

internal sealed record GetDepartmentQuery(
    int DepartmentId)
    : IRequest<Result<DepartmentResponse>>;

internal sealed class GetDepartmentQueryHandler(
    UniProDbContext dbContext)
    : IRequestHandler<GetDepartmentQuery, Result<DepartmentResponse>>
{
    public async Task<Result<DepartmentResponse>> Handle(
        GetDepartmentQuery request,
        CancellationToken cancellationToken)
    {
        var department = await dbContext
            .Departments
            .Include(x => x.StudentGroups)
            .FirstOrDefaultAsync(x => x.DepartmentId == request.DepartmentId, cancellationToken);

        return department is null 
            ? Result.Fail(new NotFoundError($"Department with ID {request.DepartmentId} not found.")) 
            : Result.Ok(department.Adapt<DepartmentResponse>());
    }
}

public sealed class GetDepartmentEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/departments/{departmentId:int}", Handler)
            .Produces<DepartmentResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithName("GetDepartment")
            .WithTags(EndpointTags.Departments);
    }

    private static async Task<IResult> Handler(
        [FromRoute] int departmentId,
        [FromServices] HttpContext ctx,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var query = new GetDepartmentQuery(departmentId);
        var result = await sender.Send(query, cancellationToken);
        
        return result.IsSuccess ? 
            Results.Ok(result.Value) : 
            result.Errors.ToProblem();
    }
}
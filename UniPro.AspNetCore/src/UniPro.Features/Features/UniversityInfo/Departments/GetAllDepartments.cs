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
using UniPro.Features.Common.Responses;
using UniPro.Features.Extensions;
using UniPro.Infrastructure.Database;

namespace UniPro.Features.Features.UniversityInfo.Departments;

internal sealed record GetAllDepartmentsQuery()
    : IRequest<Result<List<DepartmentResponse>>>;

internal sealed class GetAllDepartmentsQueryHandler(
    UniProDbContext dbContext)
    : IRequestHandler<GetAllDepartmentsQuery, Result<List<DepartmentResponse>>>
{
    public async Task<Result<List<DepartmentResponse>>> Handle(
        GetAllDepartmentsQuery request,
        CancellationToken cancellationToken)
    {
        var departments = await dbContext
            .Departments
            .ProjectToType<DepartmentResponse>()
            .ToListAsync(cancellationToken);

        return departments;
    }
}

public sealed class GetAllDepartmentsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/departments", Handler)
            .Produces<List<DepartmentResponse>>()
            .WithTags(EndpointTags.Departments);
    }

    private static async Task<IResult> Handler(
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var query = new GetAllDepartmentsQuery();
        var result = await sender.Send(query, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.Errors.ToProblem();
    }
}
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

namespace UniPro.Features.Features.UniversityInfo.StudentGroups;

internal sealed record GetAllStudentGroupsQuery()
    : IRequest<Result<List<StudentGroupResponse>>>;

internal sealed class GetAllStudentGroupsQueryHandler(
    UniProDbContext dbContext)
    : IRequestHandler<GetAllStudentGroupsQuery, Result<List<StudentGroupResponse>>>
{
    public async Task<Result<List<StudentGroupResponse>>> Handle(
        GetAllStudentGroupsQuery request,
        CancellationToken cancellationToken)
    {
        var studentGroups = await dbContext
            .StudentGroups
            .ProjectToType<StudentGroupResponse>()
            .ToListAsync(cancellationToken);

        return studentGroups;
    }
}

public sealed class GetAllStudentGroupsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/student-groups", Handler)
            .Produces<List<StudentGroupResponse>>()
            .WithTags(EndpointTags.StudentGroups);
    }

    private static async Task<IResult> Handler(
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var query = new GetAllStudentGroupsQuery();
        var result = await sender.Send(query, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.Errors.ToProblem();
    }
}
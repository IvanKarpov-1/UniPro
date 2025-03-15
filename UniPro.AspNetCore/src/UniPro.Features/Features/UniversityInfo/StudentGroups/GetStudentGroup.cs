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

namespace UniPro.Features.Features.UniversityInfo.StudentGroups;

public sealed record GetStudentGroupQuery(
    int StudentGroupId)
    : IRequest<Result<StudentGroupResponse>>;

public sealed class GetStudentGroupQueryHandler(
    UniProDbContext dbContext)
    : IRequestHandler<GetStudentGroupQuery, Result<StudentGroupResponse>>
{
    public async Task<Result<StudentGroupResponse>> Handle(
        GetStudentGroupQuery request,
        CancellationToken cancellationToken)
    {
        var studentGroup = await dbContext
            .StudentGroups
            .Include(x => x.StudentInfos)
            .ThenInclude(x => x.Student)
            .FirstOrDefaultAsync(x => x.StudentGroupId == request.StudentGroupId, cancellationToken);

        return studentGroup is not null
            ? Result.Ok(studentGroup.Adapt<StudentGroupResponse>())
            : Result.Fail(new NotFoundError($"Student group with ID {request.StudentGroupId} not found."));
    }
}

public sealed class GetStudentGroupEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/student-groups/{studentGroupId:int}", Handler)
            .RequireAuthorization()
            .Produces<StudentGroupResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithName("GetStudentGroup")
            .WithTags(EndpointTags.StudentGroups);
    }

    private static async Task<IResult> Handler(
        [FromRoute] int studentGroupId,
        [FromServices] IHttpContextAccessor ctx,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var query = new GetStudentGroupQuery(studentGroupId);
        var result = await sender.Send(query, cancellationToken);
        
        return result.IsSuccess ? 
            Results.Ok(result.Value) : 
            result.Errors.ToProblem();
    }
}
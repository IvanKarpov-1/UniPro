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

namespace UniPro.Features.Features.StudentInfo;

internal sealed record GetStudentInfoQuery(
    string StudentId)
    : IRequest<Result<StudentInfoResponse>>;

internal sealed class GetStudentInfoQueryHandler(
    UniProDbContext dbContext)
    : IRequestHandler<GetStudentInfoQuery, Result<StudentInfoResponse>>
{
    public async Task<Result<StudentInfoResponse>> Handle(
        GetStudentInfoQuery request,
        CancellationToken cancellationToken)
    {
        var studentInfo = await dbContext
            .StudentInfos
            .Include(x => x.Student)
            .Include(x => x.University)
            .Include(x => x.Academic)
            .Include(x => x.Department)
            .Include(x => x.StudentGroup)
            .FirstOrDefaultAsync(x => x.StudentId == request.StudentId, cancellationToken);

        return studentInfo is null 
            ? Result.Fail(new NotFoundError($"Student with ID {request.StudentId} not found.")) 
            : Result.Ok(studentInfo.Adapt<StudentInfoResponse>());
    }
}

public sealed class GetStudentInfoEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/student-infos/{studentId}", Handler)
            .RequireAuthorization()
            .Produces<StudentInfoResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithName("GetStudentInfo")
            .WithTags(EndpointTags.StudentInfos);
    }

    private static async Task<IResult> Handler(
        [FromRoute] string studentId,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var query = new GetStudentInfoQuery(studentId);
        var result = await sender.Send(query, cancellationToken);
        
        return result.IsSuccess ? 
            Results.Ok(result.Value) : 
            result.Errors.ToProblem();
    }
}
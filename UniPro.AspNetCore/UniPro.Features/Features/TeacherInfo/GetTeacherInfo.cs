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

namespace UniPro.Features.Features.TeacherInfo;

internal sealed record GetTeacherInfoQuery(
    string TeacherId)
    : IRequest<Result<TeacherInfoResponse>>;

internal sealed class GetTeacherInfoQueryHandler(
    UniProDbContext dbContext)
    : IRequestHandler<GetTeacherInfoQuery, Result<TeacherInfoResponse>>
{
    public async Task<Result<TeacherInfoResponse>> Handle(
        GetTeacherInfoQuery request,
        CancellationToken cancellationToken)
    {
        var teacherInfo = await dbContext
            .TeacherInfos
            .Include(x => x.Teacher)
            .Include(x => x.University)
            .Include(x => x.Academic)
            .Include(x => x.Department)
            .FirstOrDefaultAsync(x => x.TeacherId == request.TeacherId, cancellationToken);

        return teacherInfo is null 
            ? Result.Fail(new NotFoundError($"Teacher with ID {request.TeacherId} not found.")) 
            : Result.Ok(teacherInfo.Adapt<TeacherInfoResponse>());
    }
}

public sealed class GetTeacherInfoEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/teacher-infos/{teacherId}", Handler)
            .RequireAuthorization()
            .Produces<TeacherInfoResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithName("GetTeacherInfo")
            .WithTags(EndpointTags.TeacherInfos);
    }

    private static async Task<IResult> Handler(
        [FromRoute] string teacherId,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var query = new GetTeacherInfoQuery(teacherId);
        var result = await sender.Send(query, cancellationToken);
        
        return result.IsSuccess ? 
            Results.Ok(result.Value) : 
            result.Errors.ToProblem();
    }
}
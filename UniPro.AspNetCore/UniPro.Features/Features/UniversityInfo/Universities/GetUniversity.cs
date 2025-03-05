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

namespace UniPro.Features.Features.UniversityInfo.Universities;

internal sealed record GetUniversityQuery(
    int UniversityId)
    : IRequest<Result<UniversityResponse>>;

internal sealed class GetUniversityQueryHandler(
    UniProDbContext dbContext)
    : IRequestHandler<GetUniversityQuery, Result<UniversityResponse>>
{
    public async Task<Result<UniversityResponse>> Handle(
        GetUniversityQuery request,
        CancellationToken cancellationToken)
    {
        var university = await dbContext
            .Universities
            .Include(x => x.Academics)
            .FirstOrDefaultAsync(x => x.UniversityId == request.UniversityId, cancellationToken);

        return university is null 
            ? Result.Fail(new NotFoundError($"University with ID {request.UniversityId} not found.")) 
            : Result.Ok(university.Adapt<UniversityResponse>());
    }
}

public sealed class GetUniversityEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/universities/{universityId:int}", Handler)
            .Produces<UniversityResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithName("GetUniversity")
            .WithTags(EndpointTags.Universities);
    }

    private static async Task<IResult> Handler(
        [FromRoute] int universityId,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var query = new GetUniversityQuery(universityId);
        var result = await sender.Send(query, cancellationToken);
        
        return result.IsSuccess ? 
            Results.Ok(result.Value) : 
            result.Errors.ToProblem();
    }
}
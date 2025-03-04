using Carter;
using FluentResults;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using UniPro.Features.Common;
using UniPro.Features.Common.Responses;
using UniPro.Features.Extensions;
using UniPro.Infrastructure.Database;

namespace UniPro.Features.Features.UniversityInfo.Universities;

internal sealed record GetAllUniversitiesQuery()
    : IRequest<Result<List<UniversityResponse>>>;

internal sealed class GetAllUniversitiesQueryHandler(
UniProDbContext dbContext)
    : IRequestHandler<GetAllUniversitiesQuery, Result<List<UniversityResponse>>>
{
    public async Task<Result<List<UniversityResponse>>> Handle(GetAllUniversitiesQuery request, CancellationToken cancellationToken)
    {
        var universities = await dbContext
            .Universities
            .ProjectToType<UniversityResponse>()
            .ToListAsync(cancellationToken);
        
        return universities;
    }
}

public sealed class GetAllUniversitiesEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/universities", Handler)
            .Produces<List<UniversityResponse>>()
            .WithTags(EndpointTags.Universities);
    }

    private static async Task<IResult> Handler(
        ISender sender,
        CancellationToken cancellationToken)
    {
        var query = new GetAllUniversitiesQuery();
        var result = await sender.Send(query, cancellationToken);

        return result.IsSuccess ? 
            Results.Ok(result.Value) : 
            result.Errors.ToProblem();
    }
}
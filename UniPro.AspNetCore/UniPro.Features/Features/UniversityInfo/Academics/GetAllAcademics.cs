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

namespace UniPro.Features.Features.UniversityInfo.Academics;

internal sealed record GetAllAcademicsQuery()
    : IRequest<Result<List<AcademicResponse>>>;

internal sealed class GetAllAcademicsQueryHandler(
    UniProDbContext dbContext)
    : IRequestHandler<GetAllAcademicsQuery, Result<List<AcademicResponse>>>
{
    public async Task<Result<List<AcademicResponse>>> Handle(
        GetAllAcademicsQuery request,
        CancellationToken cancellationToken)
    {
        var academics = await dbContext
            .Academics
            .ProjectToType<AcademicResponse>()
            .ToListAsync(cancellationToken);

        return academics;
    }
}

public sealed class GetAllAcademicsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/academics", Handler)
            .Produces<List<AcademicResponse>>()
            .WithTags(EndpointTags.Academics);
    }

    private static async Task<IResult> Handler(
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var query = new GetAllAcademicsQuery();
        var result = await sender.Send(query, cancellationToken);

        return result.IsSuccess ? Results.Ok(result.Value) : result.Errors.ToProblem();
    }
}
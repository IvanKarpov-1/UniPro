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

namespace UniPro.Features.Features.UniversityInfo.Academics;

internal sealed record GetAcademicQuery(
    int AcademicId)
    : IRequest<Result<AcademicResponse>>;

internal sealed class GetAcademicQueryHandler(
    UniProDbContext dbContext)
    : IRequestHandler<GetAcademicQuery, Result<AcademicResponse>>
{
    public async Task<Result<AcademicResponse>> Handle(
        GetAcademicQuery request,
        CancellationToken cancellationToken)
    {
        var academic = await dbContext
            .Academics
            .Include(x => x.Departments)
            .FirstOrDefaultAsync(x => x.AcademicId == request.AcademicId, 
            cancellationToken);

        return academic is not null
            ? Result.Ok(academic.Adapt<AcademicResponse>())
            : Result.Fail(new NotFoundError($"Academic with ID {request.AcademicId} not found."));
    }
}

public sealed class GetAcademicEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/academics/{academicId:int}", Handler)
            .Produces<AcademicResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithName("GetAcademic")
            .WithTags(EndpointTags.Academics);
    }

    private static async Task<IResult> Handler(
        [FromRoute] int academicId,
        [FromServices] IHttpContextAccessor ctx,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var query = new GetAcademicQuery(academicId);
        var result = await sender.Send(query, cancellationToken);
        
        return result.IsSuccess ? 
            Results.Ok(result.Value) : 
            result.Errors.ToProblem();
    }
}
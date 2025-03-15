using Carter;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using UniPro.Features.Common;
using UniPro.Features.Common.Errors;
using UniPro.Features.Extensions;
using UniPro.Infrastructure.Database;

namespace UniPro.Features.Features.UniversityInfo.Universities;

public sealed record UpdateUniversityRequest(
    string NewUniversityName);

public sealed record UpdateUniversityCommand(
    int UniversityId,
    string NewUniversityName)
    : IRequest<Result<Unit>>;

public sealed class UpdateUniversityCommandHandler(
    UniProDbContext dbContext)
    : IRequestHandler<UpdateUniversityCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(UpdateUniversityCommand request, CancellationToken cancellationToken)
    {
        var university = await dbContext.Universities.FirstOrDefaultAsync(
            x => x.UniversityId == request.UniversityId, 
            cancellationToken);

        if (university is null)
        {
            return Result.Fail(new NotFoundError($"University with ID {request.UniversityId} not found."));
        }
        
        university.Name = request.NewUniversityName;
        
        var result = await dbContext.SaveChangesAsync(cancellationToken);
        
        return result > 0
            ? Result.Ok()
            : Result.Fail(new InternalServerError());
    }
}

public sealed class UpdateUniversityEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/universities/{universityId:int}", Handler)
            .RequireAuthorization()
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithTags(EndpointTags.Universities);
    }

    private static async Task<IResult> Handler(
        [FromRoute] int universityId,
        [FromBody] UpdateUniversityRequest request,
        [FromServices] IHttpContextAccessor ctx,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new UpdateUniversityCommand(universityId, request.NewUniversityName);
        var result = await sender.Send(command, cancellationToken);

        return result.IsSuccess ? 
            Results.Ok() : 
            result.Errors.ToProblem();
    }
}
using Carter;
using FluentResults;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using UniPro.Domain.Entities;
using UniPro.Features.Common;
using UniPro.Features.Common.Errors;
using UniPro.Features.Common.Responses;
using UniPro.Features.Extensions;
using UniPro.Infrastructure.Database;
using InternalServerError = UniPro.Features.Common.Errors.InternalServerError;

namespace UniPro.Features.Features.UniversityInfo.Universities;

public sealed record AddUniversityRequest(
    string UniversityName);

public sealed record AddUniversityCommand(
    string UniversityName)
    : IRequest<Result<UniversityResponse>>;

public sealed class AddUniversityCommandHandler(
    UniProDbContext dbContext) 
    : IRequestHandler<AddUniversityCommand, Result<UniversityResponse>>
{
    public async Task<Result<UniversityResponse>> Handle(
        AddUniversityCommand request,
        CancellationToken cancellationToken)
    {
        var existentUniversity = await dbContext.Universities.FirstOrDefaultAsync(
            x => x.Name == request.UniversityName, 
            cancellationToken);

        if (existentUniversity is not null)
        {
            return Result.Fail(new AlreadyExistError($"University with name \"{request.UniversityName}\" already exists."));
        }

        var newUniversity = new University
        {
            Name = request.UniversityName,
        };
        
        await dbContext.Universities.AddAsync(newUniversity, cancellationToken);
        
        var result = await dbContext.SaveChangesAsync(cancellationToken);
        
        return result > 0
            ? Result.Ok(newUniversity.Adapt<UniversityResponse>())
            : Result.Fail(new InternalServerError());
    }
}

public sealed class AddUniversityEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/universities", Handler)
            .RequireAuthorization()
            .Produces<UniversityResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithTags(EndpointTags.Universities);
    }

    private static async Task<IResult> Handler(
        [FromBody] AddUniversityRequest request,
        [FromServices] IHttpContextAccessor ctx,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var command = request.Adapt<AddUniversityCommand>();
        var result = await sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? Results.CreatedAtRoute(
                "GetUniversity",
                new { UniversityId = result.Value.UniversityId },
                result.Value)
            : result.Errors.ToProblem();
    }
}
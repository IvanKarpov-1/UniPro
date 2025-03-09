using Carter;
using FluentResults;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using UniPro.Features.Common;
using UniPro.Features.Common.Errors;
using UniPro.Features.Extensions;
using UniPro.Infrastructure.Database;

namespace UniPro.Features.Features.Users;

public sealed record UpdateUserRequest(
    string? Avatar,
    string? PhoneNumber);

internal record UpdateUserCommand(
    string? Avatar,
    string? PhoneNumber)
    : IRequest<Result<Unit>>
{
    public string? UserId { get; set; }
}

internal class UpdateUserCommandHandler(
    UniProDbContext dbContext)
    : IRequestHandler<UpdateUserCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users.FindAsync([request.UserId], cancellationToken);

        if (user is null)
        {
            return Result.Fail(new NotFoundError($"User with ID {request.UserId} not found."));
        }

        if (request.Avatar is not null)
        {
            user.Avatar = request.Avatar;
        }

        if (request.PhoneNumber is not null)
        {
            user.PhoneNumber = request.PhoneNumber;
        }
        
        var result = await dbContext.SaveChangesAsync(cancellationToken);
        
        return result > 0
            ? Result.Ok()
            : Result.Fail(new InternalServerError());
    }
}

public sealed class UpdateUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/users/{userId}", Handler)
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithTags(EndpointTags.Users);
    }

    private static async Task<IResult> Handler(
        [FromRoute] string userId,
        [FromBody] UpdateUserRequest request,
        [FromServices] IHttpContextAccessor httpContextAccessor,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        if (httpContextAccessor.HttpContext is null)
        {
            return Results.Problem(statusCode: StatusCodes.Status500InternalServerError);
        }
        
        var sub = httpContextAccessor.HttpContext.GetNameIdentifier();

        if (sub is null)
        {
            return Results.Problem(statusCode: StatusCodes.Status401Unauthorized);
        }

        if (sub != userId)
        {
            return Results.Problem(statusCode: StatusCodes.Status403Forbidden);
        }

        var config = new TypeAdapterConfig();
        config.ForType<UpdateUserRequest, UpdateUserCommand>().MapToConstructor(true);
        
        var command = request.Adapt<UpdateUserCommand>(config);
        command.UserId = userId;
        var result = await sender.Send(command, cancellationToken);
        
        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.Errors.ToProblem();
    }
}
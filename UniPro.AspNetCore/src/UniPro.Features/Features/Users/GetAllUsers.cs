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

namespace UniPro.Features.Features.Users;

public sealed record GetAllUsersQuery()
    : IRequest<Result<List<UserResponse>>>;

public sealed class GetAllUsersQueryHandler(
    UniProDbContext dbContext)
    : IRequestHandler<GetAllUsersQuery, Result<List<UserResponse>>>
{
    public async Task<Result<List<UserResponse>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await dbContext
            .Users
            .ProjectToType<UserResponse>()
            .ToListAsync(cancellationToken);
        
        return users;
    }
}

public sealed class GetAllUsersEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/users", Handler)
            .RequireAuthorization()
            .Produces<List<UserResponse>>()
            .WithTags(EndpointTags.Users);
    }

    private static async Task<IResult> Handler(
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var query = new GetAllUsersQuery();
        var result = await sender.Send(query, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.Errors.ToProblem();
    }
}
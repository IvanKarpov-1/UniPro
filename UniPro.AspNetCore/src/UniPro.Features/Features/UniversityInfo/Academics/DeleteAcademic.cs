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

namespace UniPro.Features.Features.UniversityInfo.Academics;

internal sealed record DeleteAcademicCommand(
    int AcademicId)
    : IRequest<Result<Unit>>;

internal sealed class DeleteAcademicCommandHandler(
    UniProDbContext dbContext)
    : IRequestHandler<DeleteAcademicCommand, Result<Unit>>
{
    public async  Task<Result<Unit>> Handle(DeleteAcademicCommand request, CancellationToken cancellationToken)
    {
        var academic = await dbContext.Academics.FirstOrDefaultAsync(
            x => x.AcademicId == request.AcademicId, 
            cancellationToken);

        if (academic is null)
        {
            return Result.Fail(new NotFoundError($"Academic with ID {request.AcademicId} not found."));
        }
        
        dbContext.Academics.Remove(academic);
        
        var result = await dbContext.SaveChangesAsync(cancellationToken);
        
        return result > 0
            ? Result.Ok()
            : Result.Fail(new InternalServerError());
    }
}

public sealed class DeleteAcademicEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/academics/{academicId:int}", Handler)
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithTags(EndpointTags.Academics);
    }

    private static async Task<IResult> Handler(
        [FromRoute] int academicId,
        [FromServices] IHttpContextAccessor ctx,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new DeleteAcademicCommand(academicId);
        var result = await sender.Send(command, cancellationToken);

        return result.IsSuccess ? 
            Results.StatusCode(StatusCodes.Status204NoContent) : 
            result.Errors.ToProblem();
    }
}
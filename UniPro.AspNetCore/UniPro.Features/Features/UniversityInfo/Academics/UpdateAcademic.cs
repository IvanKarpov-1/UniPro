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

public sealed record UpdateAcademicRequest(
    string NewAcademicName,
    int? NewUniversityId);

internal sealed record UpdateAcademicCommand(
    int AcademicId,
    string NewAcademicName,
    int? NewUniversityId)
    : IRequest<Result<Unit>>;

internal sealed class UpdateAcademicCommandHandler(
    UniProDbContext dbContext)
    : IRequestHandler<UpdateAcademicCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(UpdateAcademicCommand request, CancellationToken cancellationToken)
    {
        var academic = await dbContext.Academics.FirstOrDefaultAsync(
            x => x.AcademicId == request.AcademicId, 
            cancellationToken);

        if (academic is null)
        {
            return Result.Fail(new NotFoundError($"Academic with ID {request.AcademicId} not found."));
        }
        
        academic.Name = request.NewAcademicName;

        if (request.NewUniversityId is not null)
        {
            academic.UniversityId = (int)request.NewUniversityId;
        }
        
        var result = await dbContext.SaveChangesAsync(cancellationToken);
        
        return result == 0
            ? Result.Ok()
            : Result.Fail(new InternalServerError());
    }
}

public sealed class UpdateAcademicEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/academics/{academicId:int}", Handler)
            .RequireAuthorization()
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithTags(EndpointTags.Academics);
    }

    private static async Task<IResult> Handler(
        [FromRoute] int academicId,
        [FromBody] UpdateAcademicRequest request,
        [FromServices] HttpContext ctx,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new UpdateAcademicCommand(academicId, request.NewAcademicName, request.NewUniversityId);
        var result = await sender.Send(command, cancellationToken);

        return result.IsSuccess ? 
            Results.Ok() : 
            result.Errors.ToProblem();
    }
}
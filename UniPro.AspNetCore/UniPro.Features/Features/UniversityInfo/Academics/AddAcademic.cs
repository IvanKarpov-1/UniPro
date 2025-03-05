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

namespace UniPro.Features.Features.UniversityInfo.Academics;

public sealed record AddAcademicRequest(
    int UniversityId,
    string AcademicName);

internal sealed record AddAcademicCommand(
    int UniversityId,
    string AcademicName)
    : IRequest<Result<AcademicResponse>>;

internal sealed class AddAcademicCommandHandler(
    UniProDbContext dbContext) 
    : IRequestHandler<AddAcademicCommand, Result<AcademicResponse>>
{
    public async Task<Result<AcademicResponse>> Handle(
        AddAcademicCommand request,
        CancellationToken cancellationToken)
    {
        var university = await dbContext.Universities.FindAsync([request.UniversityId], cancellationToken);

        if (university is null)
        {
            return Result.Fail(new NotFoundError($"University with ID {request.UniversityId} not found."));
        }
        
        var existentAcademic = await dbContext.Academics.FirstOrDefaultAsync(
            x => x.Name == request.AcademicName, 
            cancellationToken);

        if (existentAcademic is not null)
        {
            return Result.Fail(new AlreadyExistError($"Academic with name \"{request.AcademicName}\" already exists."));
        }

        var newAcademic = new Academic
        {
            Name = request.AcademicName,
        };
        
        university.Academics.Add(newAcademic);
        
        var result = await dbContext.SaveChangesAsync(cancellationToken);
        
        return result == 0
            ? Result.Ok(newAcademic.Adapt<AcademicResponse>())
            : Result.Fail(new InternalServerError());
    }
}

public sealed class AddAcademicEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost<AcademicResponse>("/api/academics", Handler)
            .RequireAuthorization()
            .Produces<AcademicResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithTags(EndpointTags.Academics);
    }

    private static async Task<IResult> Handler(
        [FromBody] AddAcademicRequest request,
        [FromServices] HttpContext ctx,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var command = request.Adapt<AddAcademicCommand>();
        var result = await sender.Send(command, cancellationToken);

        return result.IsSuccess ? 
            Results.CreatedAtRoute("GetAcademic", new { AcademicId = result.Value.AcademicId }) : 
            result.Errors.ToProblem();
    }
}
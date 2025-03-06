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

namespace UniPro.Features.Features.UniversityInfo.Departments;

public sealed record AddDepartmentRequest(
    int AcademicId,
    string DepartmentName);

internal sealed record AddDepartmentCommand(
    int AcademicId,
    string DepartmentName)
    : IRequest<Result<DepartmentResponse>>;

internal sealed class AddDepartmentCommandHandler(
    UniProDbContext dbContext) 
    : IRequestHandler<AddDepartmentCommand, Result<DepartmentResponse>>
{
    public async Task<Result<DepartmentResponse>> Handle(
        AddDepartmentCommand request,
        CancellationToken cancellationToken)
    {
        var academic = await dbContext.Academics.FindAsync([request.AcademicId], cancellationToken);

        if (academic is null)
        {
            return Result.Fail(new NotFoundError($"Academic with ID {request.AcademicId} not found."));
        }
        
        var existentDepartment = await dbContext.Departments.FirstOrDefaultAsync(
            x => x.Name == request.DepartmentName, 
            cancellationToken);

        if (existentDepartment is not null)
        {
            return Result.Fail(new AlreadyExistError($"Department with name \"{request.DepartmentName}\" already exists."));
        }

        var newDepartment = new Department
        {
            Name = request.DepartmentName,
        };
        
        academic.Departments.Add(newDepartment);
        
        var result = await dbContext.SaveChangesAsync(cancellationToken);
        
        return result > 0
            ? Result.Ok(newDepartment.Adapt<DepartmentResponse>())
            : Result.Fail(new InternalServerError());
    }
}

public sealed class AddDepartmentEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/departments", Handler)
            .RequireAuthorization()
            .Produces<DepartmentResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithTags(EndpointTags.Departments);
    }

    private static async Task<IResult> Handler(
        [FromBody] AddDepartmentRequest request,
        [FromServices] IHttpContextAccessor ctx,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var command = request.Adapt<AddDepartmentCommand>();
        var result = await sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? Results.CreatedAtRoute(
                "GetDepartment",
                new { DepartmentId = result.Value.DepartmentId },
                result.Value) 
            : result.Errors.ToProblem();
    }
}
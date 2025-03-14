using FluentResults;
using Microsoft.AspNetCore.Http;
using UniPro.Features.Common.Errors;
using InternalServerError = UniPro.Features.Common.Errors.InternalServerError;

namespace UniPro.Features.Extensions;

public static class EndpointResultsExtensions
{
    public static IResult ToProblem(this IError error)
    {
        return CreateProblem(error);
    }

    public static IResult ToProblem(this List<IError> errors)
    {
        return errors.Count is 0 ? Results.Problem() : CreateProblem(errors);
    }
    
    private static IResult CreateProblem(IError error)
    {
        var statusCode = GetStatusCode(error);

        var errorsDict = new Dictionary<string, string[]> { { "error_message", [error.Message] } };

        foreach (var metadata in error.Metadata)
        {
            errorsDict.Add(metadata.Key, [metadata.Value.ToString() ?? string.Empty]);
        }
        
        return Results.ValidationProblem(errorsDict, statusCode: statusCode);
    }
    
    private static IResult CreateProblem(List<IError> errors)
    {
        var statusCode = GetStatusCode(errors.First());

        var errorsDict = new Dictionary<string, string[]>();
        foreach (var error in errors)
        {
            errorsDict.Add("error_message", [error.Message]);

            foreach (var metadata in error.Metadata)
            {
                errorsDict.Add(metadata.Key, [metadata.Value.ToString() ?? string.Empty]);
            }
        };

        return Results.ValidationProblem(errorsDict, statusCode: statusCode);
    }

    private static int GetStatusCode(IError? error)
    {
        return error switch
        {
            BadRequestError => StatusCodes.Status400BadRequest,
            NotFoundError => StatusCodes.Status404NotFound,
            AlreadyExistError => StatusCodes.Status409Conflict,
            InternalServerError => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError,
        };
    }
}
using FluentResults;

namespace UniPro.Features.Common.Errors;

public class InternalServerError : Error
{
    public InternalServerError()
    {
        
    }

    public InternalServerError(string message) : base(message)
    {
        
    }
}
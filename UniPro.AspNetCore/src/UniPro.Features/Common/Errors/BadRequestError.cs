using FluentResults;

namespace UniPro.Features.Common.Errors;

public class BadRequestError : Error
{
    public BadRequestError()
    {
        
    }

    public BadRequestError(string message) : base(message)
    {
        
    }
}
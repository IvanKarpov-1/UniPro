using FluentResults;

namespace UniPro.Features.Common.Errors;

public class NotFoundError : Error
{
    public NotFoundError()
    {
        
    }

    public NotFoundError(string message) : base(message)
    {
        
    }
}
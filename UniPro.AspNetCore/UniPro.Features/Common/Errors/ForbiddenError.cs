using FluentResults;

namespace UniPro.Features.Common.Errors;

public class ForbiddenError : Error
{
    public ForbiddenError()
    {
        
    }

    public ForbiddenError(string message) : base(message)
    {
        
    }
}
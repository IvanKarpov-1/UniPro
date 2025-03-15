using FluentResults;

namespace UniPro.Features.Common.Errors;

public class AlreadyExistError : Error
{
    public AlreadyExistError()
    {
        
    }

    public AlreadyExistError(string message) : base(message)
    {
        
    }
}
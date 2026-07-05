namespace Trackly.Domain.Exceptions;

public sealed class InvalidRefreshTokenStateException : DomainException
{
    public InvalidRefreshTokenStateException(string message) : base(message)
    {
    }
}

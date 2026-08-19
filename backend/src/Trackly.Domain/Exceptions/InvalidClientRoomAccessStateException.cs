namespace Trackly.Domain.Exceptions;

public sealed class InvalidClientRoomAccessStateException : DomainException
{
    public InvalidClientRoomAccessStateException(string message) : base(message)
    {
    }
}

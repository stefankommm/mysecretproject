namespace Signee.Domain.Exceptions;

public class EntityNotExistException : Exception
{
    public EntityNotExistException(string message = "") : base(message) {}
}
namespace Signee.DisplayHandler;

public interface IDisplayMessageClient
{
    Task ReceiveMessage(string message);
}
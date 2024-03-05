namespace Signee.DisplayHandler.Hubs.Contracts;

public interface IDisplayMessageClient
{
    Task ReceiveMessage(string message);
}
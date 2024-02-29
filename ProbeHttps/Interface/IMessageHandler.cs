namespace ProbeHttps.Interface;

public interface IMessageHandler
{
    event EventHandler<string>? ReceivedMessage; 
}
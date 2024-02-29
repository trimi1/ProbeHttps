using System.Net;
using System.Net.Sockets;
using ProbeHttps.Interface;

namespace ProbeHttps.Https;

public class UnicastService : IMessageHandler
{
    private readonly TcpListener _tcpListener;
    
    public event EventHandler<string>? ReceivedMessage;
    
    public UnicastService(string address, int port)
    {
        _tcpListener = new TcpListener(IPAddress.Any, port);
        _tcpListener.Start();

        Task.Run(ListenForUnicastMessages);
    }
    
    private async Task ListenForUnicastMessages()
    {
        while (true)
        {
            var client = await _tcpListener.AcceptTcpClientAsync();
            var stream = client.GetStream();

            using var reader = new StreamReader(stream);
            var message = await reader.ReadToEndAsync();
            
            Console.WriteLine($"Received unicast message: {message}");
        }
    }
}
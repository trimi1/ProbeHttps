using System.Net;
using System.Net.Sockets;
using System.Text;
using ProbeHttps.Interface;

namespace ProbeHttps.Https.Handler;

public class MulticastHandler
{
    private readonly UdpClient _udpClient;
    private readonly IPEndPoint _multicastEndPoint;
    private readonly Thread _thread;
    private readonly int _sleepTime;
    private readonly IBuilder _builder;

    public MulticastHandler(string multicastAddress, int multicastPort, int sleepTime, IBuilder builder)
    {
        _udpClient = new UdpClient();
        _multicastEndPoint = new IPEndPoint(IPAddress.Parse(multicastAddress), multicastPort);
        _thread = new Thread(ThreadLoop);
        _sleepTime = sleepTime;
        _builder = builder;
    }

    private void SendProbeMessage(string protocol, int port)
    {
        var probeMessage = _builder.BuildProbeMessage(protocol, port);
        var bytes = Encoding.ASCII.GetBytes(probeMessage);
        _udpClient.Send(bytes, bytes.Length, _multicastEndPoint);
    }

    public void SendDataMessage(string protocol, int port)
    {
        var dataMessage = _builder.BuildProbeMessage(protocol, port);
        var bytes = Encoding.ASCII.GetBytes(dataMessage);
        _udpClient.Send(bytes, bytes.Length, _multicastEndPoint);
    }

    private void ThreadLoop()
    {
        while (Thread.CurrentThread.IsAlive)
        {
            SendProbeMessage("https", 60202);
            Thread.Sleep(_sleepTime);
        }
    }

    public void StartAnnouncement()
        => _thread.Start();
}
using System.Collections.Immutable;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ProbeHttps.builder;

namespace ProbeHttps;

public class MulticastService
{
    private readonly UdpClient _udpClient;
    private readonly IPEndPoint _multicastEndPoint;
    private readonly int _unicastPort;
    private readonly Thread _thread;
    private readonly int _sleepTime;
    public MulticastService(string multicastAddress, int multicastPort, int unicastPort,int sleepTime)
    {
        _udpClient = new UdpClient();
        _multicastEndPoint = new IPEndPoint(IPAddress.Parse(multicastAddress), multicastPort);
        _unicastPort = unicastPort;
        _thread = new Thread(ThreadLoop);
        _sleepTime = sleepTime;
    }

    public void Start()
        => _thread.Start();

    private void SendProbeMessage(string protocol, int port)
    {
        var probeMessage = Builder.BuildProbe(protocol, port);
        var bytes = Encoding.ASCII.GetBytes(probeMessage);
        _udpClient.Send(bytes, bytes.Length, _multicastEndPoint);
    }

    public void SendDataMessage(string protocol, int port)
    {
        var dataMessage = Builder.BuildProbe(protocol,port);
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
}
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
    private readonly ISender _sender;

<<<<<<< HEAD
    public MulticastHandler(string multicastAddress, int multicastPort, int sleepTime, IBuilder builder,ISender sender )
=======
    public MulticastHandler(string multicastAddress, int multicastPort, int sleepTime, IBuilder builder)
>>>>>>> 69bcc778816ee0048d5c7f68547561f96fcb729d
    {
        _udpClient = new UdpClient();
        _multicastEndPoint = new IPEndPoint(IPAddress.Parse(multicastAddress), multicastPort);
        _thread = new Thread(ThreadLoop);
        _sleepTime = sleepTime;
        _builder = builder;
<<<<<<< HEAD
        _sender = sender;
=======
>>>>>>> 69bcc778816ee0048d5c7f68547561f96fcb729d
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
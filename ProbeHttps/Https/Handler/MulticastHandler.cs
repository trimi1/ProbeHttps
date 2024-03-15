using System.Net;
using System.Net.Sockets;
using System.Text;
using ProbeHttps.Config;
using ProbeHttps.Interface;
using ProbeHttps.Singleton;

namespace ProbeHttps.Https.Handler;

public class MulticastHandler
{
    private readonly Thread _thread;
    private readonly int _sleepTime;
    private readonly int _multicastPort;
    private readonly int _unicastPort;
    private readonly UdpClient _udpClient;
    private readonly IPEndPoint _multicastEndPoint;

    public MulticastHandler(int sleepTime, string multicastAddress, int multicastPort, int unicastPort, IObservable observable)
    {
        _udpClient = new UdpClient();
        _multicastEndPoint = new IPEndPoint(IPAddress.Parse(multicastAddress), multicastPort);
        _multicastPort = multicastPort;
        _unicastPort = unicastPort;
        _thread = new Thread(ThreadLoop);
        _sleepTime = sleepTime;

        observable.StateChanged += AnnounceData;
    }

    private void ThreadLoop()
    {
        while (Thread.CurrentThread.IsAlive)
        {
            AnnounceProbe();
            Thread.Sleep(_sleepTime);
        }
    }

    public void StartAnnouncement()
        => _thread.Start();

    private void AnnounceProbe()
    {
        var bytes = Encoding.ASCII.GetBytes(Builder.BuildProbeMessage(Constants.Protocol, _unicastPort));
        _udpClient.Send(bytes, bytes.Length, _multicastEndPoint);
    }

    private void AnnounceData()
    {
        var bytes = Encoding.ASCII.GetBytes(Builder.BuildDataMessage(Constants.Protocol, _unicastPort));
        _udpClient.Send(bytes, bytes.Length, _multicastEndPoint);
    }
}
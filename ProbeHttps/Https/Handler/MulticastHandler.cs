using System.Net;
using System.Net.Sockets;
using System.Text;
using ProbeHttps.Interface;

namespace ProbeHttps.Https.Handler;

public class MulticastHandler
{
    private readonly Thread _thread;
    private readonly int _sleepTime;
    private readonly ISender _sender;

    public MulticastHandler(int sleepTime, ISender sender)
    {
        _thread = new Thread(ThreadLoop);
        _sleepTime = sleepTime;
        _sender = sender;
    }

    private void SendProbeMessage(string protocol, int port)
    {
        _sender.SendProbeMessage(protocol, port);
    }

    // public void SendDataMessage(string protocol, int port)
    // {
    //     var dataMessage = _builder.BuildProbeMessage(protocol, port);
    //     var bytes = Encoding.ASCII.GetBytes(dataMessage);
    //     _udpClient.Send(bytes, bytes.Length, _multicastEndPoint);
    // }


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
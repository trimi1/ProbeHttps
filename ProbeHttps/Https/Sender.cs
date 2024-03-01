using System.Net;
using System.Net.Sockets;
using System.Text;
using ProbeHttps.Interface;

namespace ProbeHttps.Https;

public class Sender : ISender
{
    private readonly IBuilder _builder;
    private readonly UdpClient _udpClient;
    private readonly IPEndPoint _multicastEndPoint;
    public Sender(IBuilder builder, string multicastAddress, int multicastPort)
    {
        _builder = builder;
        _udpClient = new UdpClient();
        _multicastEndPoint = new IPEndPoint(IPAddress.Parse(multicastAddress), multicastPort);

    }

    public void SendProbeMessage(string protocol, int port)
    {
        var probeMessage = _builder.BuildProbeMessage(protocol, port);
        var bytes = Encoding.ASCII.GetBytes(probeMessage);
        _udpClient.Send(bytes, bytes.Length, _multicastEndPoint);    }

    public void SendDataMessage(string protocol, int port)
    {
        throw new NotImplementedException();
    }

    public void SendStatusMessage(string id, string state)
    {
        throw new NotImplementedException();
    }
}
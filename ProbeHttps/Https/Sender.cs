using ProbeHttps.Interface;

namespace ProbeHttps.Https;

public class Sender : ISender
{
    public void SendProbeMessage(string protocol, int port)
    {
        throw new NotImplementedException();
    }

    public void SendDataMessage(string protocol, int port)
    {
        throw new NotImplementedException();
    }

    public void SendStatusMessage(string id, string state)
    {
        throw new NotImplementedException();
    }
}
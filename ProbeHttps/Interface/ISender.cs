namespace ProbeHttps.Interface;

public interface ISender
{
    void SendProbeMessage(string protocol, int port);
    void SendDataMessage(string protocol, int port);
    void SendStatusMessage(string id, string state);
}
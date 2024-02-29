namespace ProbeHttps.Interface;

public interface IBuilder
{
    string BuildProbeMessage(string protocol, int port);
    string BuildDataMessage(string protocol, int port);
}
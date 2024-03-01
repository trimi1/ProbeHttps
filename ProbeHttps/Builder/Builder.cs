using ProbeHttps.Interface;

namespace ProbeHttps.Builder;

public class Builder : IBuilder
{
    private const string Probe = "PROBE <protocol> <port>\n\n";
    private const string Data = "DATA <protocol> <port>\n\n";
    private const string Status = "STATUS <id> <state>\n\n";
    private static string ReplacePlaceholder(string template, string placeholder, string value)
        => template.Replace("<" + placeholder + ">", value);

    public string BuildProbeMessage(string protocol, int port)
        => ReplacePlaceholder(ReplacePlaceholder(Probe, "protocol", protocol), "port", port.ToString());
    
    public  string BuildDataMessage(string protocol, int port)
        => ReplacePlaceholder(ReplacePlaceholder(Data, "protocol", protocol), "port", port.ToString());

    public string BuildStatusMessage(int id, string state)
    => ReplacePlaceholder(ReplacePlaceholder(Status, "id", id.ToString()), "state", state);
}
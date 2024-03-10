using ProbeHttps.Interface;

namespace ProbeHttps.Singleton;

public static class Builder
{
    private const string Probe = "PROBE <protocol> <port>\r\n";
    private const string Data = "DATA <protocol> <port>\r\n";
    private const string Status = "STATUS <id> <state>\r\n";

    private static string ReplacePlaceholder(string template, string placeholder, string value)
        => template.Replace("<" + placeholder + ">", value);

    public static string BuildProbeMessage(string protocol, int port)
        => ReplacePlaceholder(ReplacePlaceholder(Probe, "protocol", protocol), "port", port.ToString());

    public static string BuildDataMessage(string protocol, int port)
        => ReplacePlaceholder(ReplacePlaceholder(Data, "protocol", protocol), "port", port.ToString());

    public static string BuildStatusMessage(string id, string state)
        => ReplacePlaceholder(ReplacePlaceholder(Status, "id", id), "state", state);
}
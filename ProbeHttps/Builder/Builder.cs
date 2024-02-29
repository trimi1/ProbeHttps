namespace ProbeHttps.Builder;

public static class Builder
{
    private const string Probe = "PROBE <protocol> <port>\n\n";
    private const string Data = "DATA <protocol> <port>\n\n";

    private static string ReplacePlaceholder(string template, string placeholder, string value)
        => template.Replace("<" + placeholder + ">", value);

    public static string BuildProbe(string protocol, int port)
        => ReplacePlaceholder(ReplacePlaceholder(Probe, "protocol", protocol), "port", port.ToString());


    public static string BuildData(string protocol, int port)
        => ReplacePlaceholder(ReplacePlaceholder(Data, "protocol", protocol), "port", port.ToString());
}
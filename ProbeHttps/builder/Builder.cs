namespace ProbeHttps.builder;

public class Builder
{

    public static string Probe = "PROBE <protocol> <port>\n";
    public static string Data = "DATA <protocol> <port>\n";

    private static string ReplacePlaceholder(string template, string placeholder, string value) {
        return template.Replace("<" + placeholder + ">", value);
    }
    public static string BuildProbe(string protocol,int port) {
        return ReplacePlaceholder(ReplacePlaceholder(Probe, "protocol", protocol), "port", port.ToString());
    }
    public static string BuildData(string protocol, int port) {
        return ReplacePlaceholder(ReplacePlaceholder(Data, "protocol", protocol), "port", port.ToString());
    }
}
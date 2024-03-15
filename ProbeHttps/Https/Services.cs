namespace ProbeHttps.Https;

public static class Services
{
    public static int Frequency { get; set; } = 10000;
    public static List<string> Urls { get; } = new();
    public static List<Service> ServicesList { get; } = new();
}

public class Service
{
    public string Id { get; }
    public string Url { get; }
    public int ExpectedStatusCode { get; }
    public int MaxResponseTime { get; }
    public State State { get; set; }

    public Service(string id, string url, int expectedStatusCode, int maxResponseTime, State state = State.Unknown)
    {
        Id = id;
        Url = url;
        ExpectedStatusCode = expectedStatusCode;
        MaxResponseTime = maxResponseTime;
        State = state;
    }
}

public enum State
{
    Ok,
    Alarm,
    Down,
    Unknown
}
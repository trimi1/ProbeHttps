namespace ProbeHttps.Https;

public static class Services
{
    public static int Frequency { get; set; } = 30000;
    public static List<Service> ServicesList { get; } = new();
}

public class Service
{
    public string Id { get; set; }
    public int Frequency { get; set; }
    public string Url { get; set; }
    public int ExpectedStatusCode { get; set; }
    public int MaxResponseTime { get; set; }
    public State State { get; set; }

    public Service(string id,int frequency, string url, int expectedStatusCode, int maxResponseTime, State state = State.Unknown)
    {
        Id = id;
        Frequency = frequency;
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
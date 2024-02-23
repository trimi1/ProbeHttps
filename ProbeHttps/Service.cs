namespace ProbeHttps;

public record Service(string Id, string Url, int ExpectedStatusCode, int MaxResponseTime);
using System.Diagnostics;

namespace ProbeHttps;

public class Probe
{
    private readonly HttpClient _client;

    public Probe()
    {
        var handler = new HttpClientHandler();
        handler.AllowAutoRedirect = false;
        _client = new HttpClient(handler);
    }

    public async Task CheckService(string url, int expectedStatusCode, int maxResponseTime)
    {
        var stopwatch = Stopwatch.StartNew();
        var response = await _client.GetAsync(url);
        stopwatch.Stop();

        if ((int)response.StatusCode != expectedStatusCode)
        {
            Console.WriteLine(
                $"Service at {url} is DOWN. Expected HTTP response code {expectedStatusCode}, but got {(int)response.StatusCode}.");
        }
        else if (stopwatch.ElapsedMilliseconds > maxResponseTime)
        {
            Console.WriteLine(
                $"Service at {url} is in ALARM state. Response time {stopwatch.ElapsedMilliseconds}ms exceeded the maximum allowed {maxResponseTime}ms.");
        }
        else
        {
            Console.WriteLine(
                $"Service at {url} is UP. HTTP response code: {(int)response.StatusCode}. Response time: {stopwatch.ElapsedMilliseconds}ms.");
        }
    }
}
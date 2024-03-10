using System.Diagnostics;
using ProbeHttps.Interface;

namespace ProbeHttps.Https.Handler;

public class ServiceHandler : IObservable
{
    private readonly Thread _thread;
    private readonly HttpClient _client;
    public event Action? StateChanged;

    public ServiceHandler()
    {
        _thread = new Thread(ThreadLoop);
        var handler = new HttpClientHandler();
        handler.AllowAutoRedirect = false;
        _client = new HttpClient(handler);
    }

    private void ThreadLoop()
    {
        while (Thread.CurrentThread.IsAlive)
        {
            CheckServices();
            Thread.Sleep(Services.Frequency);
        }
    }

    public void StartMonitoring()
        => _thread.Start();

    private void CheckServices()
    {
        var anyStateChanged = false;

        foreach (var service in Services.ServicesList)
        {
            var previousState = service.State;
            CheckService(service).Wait();
            if (service.State != previousState)
            {
                anyStateChanged = true;
            }
        }

        if (anyStateChanged) StateChanged?.Invoke();
    }

    private async Task CheckService(Service service)
    {
        var stopwatch = Stopwatch.StartNew();
        var response = await _client.GetAsync(service.Url);
        stopwatch.Stop();

        if ((int)response.StatusCode != service.ExpectedStatusCode)
        {
            service.State = State.Down;
            Console.WriteLine(
                $"Service at {service.Url} is DOWN. Expected HTTP response code {service.ExpectedStatusCode}, but got {(int)response.StatusCode}.");
        }
        else if (stopwatch.ElapsedMilliseconds > service.MaxResponseTime)
        {
            service.State = State.Alarm;
            Console.WriteLine(
                $"Service at {service.Url} is in ALARM state. Response time {stopwatch.ElapsedMilliseconds}ms exceeded the maximum allowed {service.MaxResponseTime}ms.");
        }
        else
        {
            service.State = State.Ok;
            Console.WriteLine(
                $"Service at {service.Url} is OK. HTTP response code: {(int)response.StatusCode}. Response time: {stopwatch.ElapsedMilliseconds}ms.");
        }
    }
}
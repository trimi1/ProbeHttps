using ProbeHttps.Config;
using ProbeHttps.Interface;

namespace ProbeHttps.Https;

public class Probe
{
    private readonly IMessageHandler _messageHandler;
    private readonly Configuration _config;
    private readonly IAnalyzer _analyzer;

    private delegate object? AnalyzeAction(string message);

    private readonly IDictionary<int, AnalyzeAction> _analyzeActions;
    // private readonly HttpClient _client;

    public Probe(IMessageHandler messageHandler, Configuration config, IAnalyzer analyzer)
    {
        _messageHandler = messageHandler;
        _messageHandler.ReceivedMessage += OnReceivedMessage;
        _config = config;
        _analyzer = analyzer;
        _analyzeActions = new Dictionary<int, AnalyzeAction>
        {
            { 0, _analyzer.HandleSetupMessage },
            { 1, _analyzer.HandleStatusOfMessage },
            { 2, _analyzer.HandleStatusMessage }
        };
        
        OnReceivedMessage("",
            "SETUP 10000 http1!https://www.swilabus.com/!0!1500 http1!https://www.swilabus.com/!0!1500\r\n");
        OnReceivedMessage("", "STATUSOF 12345\r\n");
        OnReceivedMessage("", "STATUS 12345 OK\r\n");
        // var handler = new HttpClientHandler();
        // handler.AllowAutoRedirect = false;
        // _client = new HttpClient(handler);
    }

    private void OnReceivedMessage(object? sender, string e)
    {
        var index = _analyzer.Analyze(e);
        if (_analyzeActions.TryGetValue(index, out var action))
        {
            var result = action(e);
        }
        else
        {
            Console.WriteLine("Unknown message received.");
        }
    }

    // public async Task CheckService(string url, int expectedStatusCode, int maxResponseTime)
    // {
    //     var stopwatch = Stopwatch.StartNew();
    //     var response = await _client.GetAsync(url);
    //     stopwatch.Stop();
    //
    //     if ((int)response.StatusCode != expectedStatusCode)
    //     {
    //         Console.WriteLine(
    //             $"Service at {url} is DOWN. Expected HTTP response code {expectedStatusCode}, but got {(int)response.StatusCode}.");
    //     }
    //     else if (stopwatch.ElapsedMilliseconds > maxResponseTime)
    //     {
    //         Console.WriteLine(
    //             $"Service at {url} is in ALARM state. Response time {stopwatch.ElapsedMilliseconds}ms exceeded the maximum allowed {maxResponseTime}ms.");
    //     }
    //     else
    //     {
    //         Console.WriteLine(
    //             $"Service at {url} is UP. HTTP response code: {(int)response.StatusCode}. Response time: {stopwatch.ElapsedMilliseconds}ms.");
    //     }
    // }
}
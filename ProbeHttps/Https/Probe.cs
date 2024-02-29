using ProbeHttps.Config;
using ProbeHttps.Https.Handler;
using ProbeHttps.Https.Strategy;
using ProbeHttps.Interface;

namespace ProbeHttps.Https;

public class Probe
{
    private readonly IMessageHandler _messageHandler;
    private readonly Configuration _config;
    private readonly IAnalyzer _analyzer;
    private readonly IDictionary<int, IMessageStrategy> _messageStrategies;

    public Probe(IMessageHandler messageHandler, Configuration config, IAnalyzer analyzer, ServiceHandler serviceHandler)
    {
        _messageHandler = messageHandler;
        _messageHandler.ReceivedMessage += OnReceivedMessage;
        _config = config;
        _analyzer = analyzer;
        _messageStrategies = new Dictionary<int, IMessageStrategy>
        {
            { 0, new SetupMessageStrategy(_analyzer) },
            { 1, new StatusOfMessageStrategy(_analyzer) },
            { 2, new StatusMessageStrategy(_analyzer) }
        };
        
        OnReceivedMessage("",
            "SETUP 10000 http1!https://www.swilabus.com/!200!1500\r\n");
        serviceHandler.StartMonitoring();
        
        // OnReceivedMessage("", "STATUSOF 12345\r\n");
        // OnReceivedMessage("", "STATUS 12345 OK\r\n");
    }

    private void OnReceivedMessage(object? sender, string e)
    {
        var index = _analyzer.Analyze(e);
        if (_messageStrategies.TryGetValue(index, out var strategy))
        {
           strategy.HandleMessage(e);
        }
        else
        {
            Console.WriteLine("Unknown message received.");
        }
    }
}
using ProbeHttps.Config;
using ProbeHttps.Https.Handler;
using ProbeHttps.Https.Strategy;
using ProbeHttps.Interface;

namespace ProbeHttps.Https;

public class Probe
{
    private readonly IAnalyzer _analyzer;
    private readonly IMessageHandler _messageHandler;
    private readonly IDictionary<int, IMessageStrategy> _messageStrategies;
    private readonly ServiceHandler _serviceHandler;
    private readonly MulticastHandler _multicastHandler;

    public Probe(Configuration config, ServiceHandler serviceHandler, IAnalyzer analyzer, IBuilder builder)
    {
        _analyzer = analyzer;
        _messageHandler = new UnicastHandler(config.MulticastAddress, config.UnicastPort);
        _messageHandler.ReceivedMessage += OnReceivedMessage;
        _messageStrategies = new Dictionary<int, IMessageStrategy>
        {
            { 0, new SetupMessageStrategy() },
            { 1, new StatusOfMessageStrategy() },
            { 2, new StatusMessageStrategy() }
        };
        _multicastHandler =
            new MulticastHandler(config.MulticastAddress, config.MulticastPort, config.MulticastDelay, builder);
        _serviceHandler = serviceHandler;

        OnReceivedMessage("", "SETUP 10000 http1!https://www.swilabus.com/!200!1500\r\n");
        OnReceivedMessage("", "STATUSOF http1\r\n");
        // OnReceivedMessage("", "STATUS 12345 OK\r\n");
    }

    public void StartServiceMonitoring()
        => _serviceHandler.StartMonitoring();

    public void StartMulticastProbeAnnouncement()
        => _multicastHandler.StartAnnouncement();

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
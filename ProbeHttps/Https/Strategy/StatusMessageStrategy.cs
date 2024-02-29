using ProbeHttps.Interface;

namespace ProbeHttps.Https.Strategy;

public class StatusMessageStrategy : IMessageStrategy
{
    private readonly IAnalyzer _analyzer;
    
    public StatusMessageStrategy(IAnalyzer analyzer)
    {
        _analyzer = analyzer;
    }
    
    public void HandleMessage(string message)
    {
        var result = _analyzer.HandleStatusMessage(message);
    }
}
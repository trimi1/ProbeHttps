using ProbeHttps.Interface;

namespace ProbeHttps.Https.Strategy;

public class StatusOfMessageStrategy : IMessageStrategy
{
    private readonly IAnalyzer _analyzer;

    public StatusOfMessageStrategy(IAnalyzer analyzer)
    {
        _analyzer = analyzer;
    }

    public void HandleMessage(string message)
    {
        var result = _analyzer.HandleStatusOfMessage(message);
    }
}
using ProbeHttps.Interface;

namespace ProbeHttps.Https.Strategy;

public class SetupMessageStrategy : IMessageStrategy
{
    private readonly IAnalyzer _analyzer;

    public SetupMessageStrategy(IAnalyzer analyzer)
    {
        _analyzer = analyzer;
    }

    public void HandleMessage(string message)
    {
        var result = _analyzer.HandleSetupMessage(message);
        if (result is null) return;

        foreach (var aurl in result.Value.Aurls)
        {
            var parts = aurl.Split('!');
            Services.ServicesList.Add(
                new Service(result.Value.Frequency, parts[1], int.Parse(parts[2]), int.Parse(parts[3]))
            );
        }
    }
}
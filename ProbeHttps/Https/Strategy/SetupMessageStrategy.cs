using System.Text.RegularExpressions;
using ProbeHttps.Config;
using ProbeHttps.Interface;

namespace ProbeHttps.Https.Strategy;

public class SetupMessageStrategy : IMessageStrategy
{
    public void HandleMessage(string message)
    {
        var result = HandleSetupMessage(message);
        if (result is null) return;

        foreach (var aurl in result.Value.Aurls)
        {
            var parts = aurl.Split('!');
            Services.ServicesList.Add(
                new Service(result.Value.Frequency, parts[1], int.Parse(parts[2]), int.Parse(parts[3]))
            );
        }
    }
    
    private static (int Frequency, IList<string> Aurls)? HandleSetupMessage(string message)
    {
        var match = Regex.Match(message, Constants.SetupRegex);
        if (!match.Success) return null;

        var frequency = match.Groups["Frequency"].Value;
        var aurs = match.Groups["Aurls"].Captures.Select(c => c.Value).ToList();

        return (Frequency: int.Parse(frequency), Aurls: aurs);
    }
}
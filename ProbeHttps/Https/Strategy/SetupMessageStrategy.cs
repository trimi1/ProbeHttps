using System.Text.RegularExpressions;
using ProbeHttps.Config;
using ProbeHttps.Interface;

namespace ProbeHttps.Https.Strategy;

public class SetupMessageStrategy : IMessageStrategy
{
    private readonly IList<string> _urlList = new List<string>();
    public void HandleMessage(string message)
    {
        var result = HandleSetupMessage(message);
        if (result is null) return;

        foreach (var aurl in result.Value.Aurls)
        {
            var parts = aurl.Split('!');
            if (_urlList.Contains(parts[1])) continue;
            Services.ServicesList.Add(
                new Service(parts[0],result.Value.Frequency, parts[1], int.Parse(parts[2]), int.Parse(parts[3]))
            );
            _urlList.Add(parts[1]);
        }
    }




    private static (int Frequency, IList<string> Aurls)? HandleSetupMessage(string message)
    {
        var match = Regex.Match(message, Constants.SetupRegex);
        if (!match.Success) return null;

        var frequency = match.Groups["Frequency"].Value;
        var aurls = match.Groups["Aurls"].Captures.Select(c => c.Value).ToList();

        return (Frequency: int.Parse(frequency), Aurls: aurls);
    }
}
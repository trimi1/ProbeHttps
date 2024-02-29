using System.Text.RegularExpressions;
using ProbeHttps.Config;
using ProbeHttps.Interface;

namespace ProbeHttps.Https.Strategy;

public class StatusOfMessageStrategy : IMessageStrategy
{
    public void HandleMessage(string message)
    {
        var result = HandleStatusOfMessage(message);
    }
    
    private static int? HandleStatusOfMessage(string message)
    {
        var match = Regex.Match(message, Constants.StatusOfRegex);
        if (!match.Success) return null;

        return int.Parse(match.Groups["Id"].Value);
    }
}
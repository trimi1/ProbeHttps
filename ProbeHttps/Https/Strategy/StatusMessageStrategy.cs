using System.Text.RegularExpressions;
using ProbeHttps.Config;
using ProbeHttps.Interface;

namespace ProbeHttps.Https.Strategy;

public class StatusMessageStrategy : IMessageStrategy
{
    public void HandleMessage(string message)
    {
        var result = HandleStatusMessage(message);
        foreach (var service in Services.ServicesList)
        {
            if (service.Id.Equals(result.Value.Id.ToString()))
            {

            }
        }
    }

    private static (int Id, string State)? HandleStatusMessage(string message)
    {
        var match = Regex.Match(message, Constants.StatusRegex);
        if (!match.Success) return null;

        return (Id: int.Parse(match.Groups["Id"].Value), State: match.Groups["State"].Value);
    }
}
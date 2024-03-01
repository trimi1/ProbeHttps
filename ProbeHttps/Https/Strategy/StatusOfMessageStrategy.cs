using System.Text.RegularExpressions;
using ProbeHttps.Config;
using ProbeHttps.Interface;

namespace ProbeHttps.Https.Strategy;

public class StatusOfMessageStrategy : IMessageStrategy
{
    private readonly ISender _sender;

    public StatusOfMessageStrategy(ISender sender)
    {
        _sender = sender;
    }

    public void HandleMessage(string message)
    {
        var result = HandleStatusOfMessage(message);
        foreach (var service in Services.ServicesList)
        {
            if (service.Id == result)
            {
                _sender.SendStatusMessage(service.Id, service.State.ToString());
            }
        }
    }


    private static string? HandleStatusOfMessage(string message)
    {
        var match = Regex.Match(message, Constants.StatusOfRegex);
        return !match.Success ? null : match.Groups["Id"].Value;
    }
}
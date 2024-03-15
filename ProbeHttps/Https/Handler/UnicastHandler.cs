using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using ProbeHttps.Config;
using ProbeHttps.Singleton;

namespace ProbeHttps.Https.Handler;

public class UnicastHandler
{
    private readonly Thread _thread;
    private readonly TcpListener _tcpListener;
    private readonly Analyzer _analyzer;

    public UnicastHandler(int port)
    {
        _thread = new Thread(ListenForUnicastMessages);
        _tcpListener = new TcpListener(IPAddress.Any, port);
        _tcpListener.Start();
        _analyzer = Analyzer.GetInstance();
    }

    public void StartListening()
        => _thread.Start();


    private void ListenForUnicastMessages()
    {
        while (true)
        {
            var client = _tcpListener.AcceptTcpClient();
            HandleClient(client);
        }
    }

    private void HandleClient(TcpClient client)
    {
        using var fromClient = new StreamReader(client.GetStream(), Encoding.UTF8);
        using var toClient = new StreamWriter(client.GetStream(), Encoding.UTF8);

        try
        {
            while (fromClient.ReadLine() is { } message)
            {
                AnalyzeAndRespond(message, toClient);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error handling client: {ex.Message}");
        }
    }

    private void AnalyzeAndRespond(string message, TextWriter toClient)
    {
        var result = _analyzer.Analyze(message);
        switch (result)
        {
            case 0:
                HandleSetupMessage(message);
                break;
            case 1:
                HandleStatusMessage(message, toClient);
                break;
        }
    }

    private static void HandleSetupMessage(string message)
    {
        var setupMatch = Regex.Match(message, Constants.SetupRegex);
        if (!setupMatch.Success) return;
        var frequency = setupMatch.Groups["Frequency"].Value;
        Services.Frequency = int.Parse(frequency) * 1000;

        var aurls = setupMatch.Groups["Aurls"].Captures.Select(c => c.Value).ToList();
        foreach (var aurl in aurls)
        {
            var aurlMatch = Regex.Match(aurl, Constants.AugmentedUrlRegex);
            if (!aurlMatch.Success) continue;

            var id = aurlMatch.Groups["Id"].Value;
            var url = aurlMatch.Groups["Url"].Value;
            var status = int.Parse(aurlMatch.Groups["Min"].Value);
            var time = int.Parse(aurlMatch.Groups["Max"].Value);

            if (Services.Urls.Contains(url)) continue;

            Services.Urls.Add(url);
            Services.ServicesList.Add(new Service(id, url, status, time));
        }
    }

    private static void HandleStatusMessage(string message, TextWriter toClient)
    {
        var match = Regex.Match(message, Constants.StatusOfRegex);
        var id = !match.Success ? null : match.Groups["Id"].Value;

        var service = Services.ServicesList.FirstOrDefault(s => s.Id == id);
        if (service == null) return;
        
        toClient.Write(Builder.BuildStatusMessage(service.Id, service.State.ToString().ToUpper()));
        toClient.Flush();
    }
}
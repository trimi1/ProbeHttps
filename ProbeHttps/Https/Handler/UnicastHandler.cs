using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using ProbeHttps.Config;
using ProbeHttps.Singleton;

namespace ProbeHttps.Https.Handler;

public class UnicastHandler
{
    private readonly TcpListener _tcpListener;

    public UnicastHandler(int port)
    {
        _tcpListener = new TcpListener(IPAddress.Any, port);
        _tcpListener.Start();
    }

    public void StartListening()
    {
        Task.Run(ListenForUnicastMessages);
    }

    private async Task ListenForUnicastMessages()
    {
        while (true)
        {
            var client = await _tcpListener.AcceptTcpClientAsync();
            await HandleClient(client);
        }
    }

    private static async Task HandleClient(TcpClient client)
    {
        using var fromClient = new StreamReader(client.GetStream(), Encoding.UTF8);
        await using var toClient = new StreamWriter(client.GetStream(), Encoding.UTF8);
        try
        {
            var message = await fromClient.ReadLineAsync();
            if (message != null)
            {
                Console.WriteLine(message);
                await AnalyzeAndRespond(message, toClient);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error handling client: {ex.Message}");
        }
        finally
        {
            client.Close();
        }
    }

    private static async Task AnalyzeAndRespond(string message, TextWriter toClient)
    {
        var analyser = Analyzer.GetInstance();
        var result = analyser.Analyze(message);

        switch (result)
        {
            case 0:
                HandleSetupMessage(message);
                break;
            case 1:
                await HandleStatusMessage(message, toClient);
                break;
        }
    }
    
    private static void HandleSetupMessage(string message)
    {
        var setupMatch = Regex.Match(message, Constants.SetupRegex);
        if (!setupMatch.Success) return;
        
        var frequency = setupMatch.Groups["Frequency"].Value;
        Services.Frequency = int.Parse(frequency);

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
    
    private static async Task HandleStatusMessage(string message, TextWriter toClient)
    {
        var match = Regex.Match(message, Constants.StatusOfRegex);
        var id = !match.Success ? null : match.Groups["Id"].Value;

        var service = Services.ServicesList.FirstOrDefault(s => s.Id == id);
        if (service != null)
        {
            await toClient.WriteAsync(Builder.BuildStatusMessage(service.Id, service.State.ToString()));
            await toClient.FlushAsync();
        }
    }
}
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
        var match = Regex.Match(message, Constants.SetupRegex);
        if (!match.Success) return;
        var frequency = match.Groups["Frequency"].Value;
        var aurls = match.Groups["Aurls"].Captures.Select(c => c.Value).ToList();

        Services.Frequency = int.Parse(frequency);

        foreach (var parts in aurls.Select(aurl => aurl.Split('!')).Where(parts => !Services.Urls.Contains(parts[1])))
        {
            Services.Urls.Add(parts[1]);
            Services.ServicesList.Add(
                new Service(
                    parts[0],
                    parts[1],
                    int.Parse(parts[2]),
                    int.Parse(parts[3])
                )
            );
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
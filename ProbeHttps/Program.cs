using ProbeHttps.Config;
using ProbeHttps.Https.Handler;
using ProbeHttps.Security;

if (!File.Exists(args[0]))
{
    Console.WriteLine("Config file not found");
    return;
}

var config = ConfigReader.GetConfig(args[0]);

if (config == null)
{
    Console.WriteLine("Config is null");
    return;
}

var serviceHandler = new ServiceHandler();
serviceHandler.StartMonitoring();

var multicastHandler = new MulticastHandler(config.MulticastDelay, config.MulticastAddress, config.MulticastPort, config.UnicastPort, serviceHandler);
multicastHandler.StartAnnouncement();

var unicastHandler = new UnicastHandler(config.UnicastPort, new AesEncryption(config.AesKey));
unicastHandler.StartListening();
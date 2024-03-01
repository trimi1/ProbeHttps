using ProbeHttps.analyzer;
using ProbeHttps.Builder;
using ProbeHttps.Config;
using ProbeHttps.Https;
using ProbeHttps.Https.Handler;

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

var probe = new Probe(config, new ServiceHandler(), new Analyzer(),new Sender(new Builder(), config.MulticastAddress, config.MulticastPort));
probe.StartServiceMonitoring();
probe.StartMulticastProbeAnnouncement();
using ProbeHttps.analyzer;
using ProbeHttps.Config;
using ProbeHttps.Https;

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

var probe = new Probe(config, new Analyzer());
probe.StartServiceMonitoring();
probe.StartMulticastProbeAnnouncement();
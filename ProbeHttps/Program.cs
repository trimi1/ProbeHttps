using ProbeHttps.analyzer;
using ProbeHttps.Builder;
using ProbeHttps.Config;
using ProbeHttps.Https;
using ProbeHttps.Https.Handler;

const string configFilePath =
    @"/Users/quentin/Desktop/Documents/Ecole/HELMo/b2/reseau/ProbeHttps/ProbeHttps/Resource/config.json";
if (!File.Exists(configFilePath))
{
    Console.WriteLine("Config file not found");
    return;
}

var config = ConfigReader.GetConfig(configFilePath);

if (config == null)
{
    Console.WriteLine("Config is null");
    return;
}

var probe = new Probe(config, new ServiceHandler(), new Analyzer(), new Builder());
probe.StartServiceMonitoring();
probe.StartMulticastProbeAnnouncement();
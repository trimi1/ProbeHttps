using ProbeHttps.analyzer;
using ProbeHttps.Config;
using ProbeHttps.Https;
using ProbeHttps.Https.Handler;

const string configFilePath = @"/Users/quentin/Desktop/Documents/Ecole/HELMo/b2/reseau/ProbeHttps/ProbeHttps/Resource/config.json";
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

<<<<<<< HEAD
var multicastService = new MulticastService(config.MulticastAddress, config.MulticastPort, config.UnicastPort,config.MulticastDelay);
multicastService.Start();

=======
var probe = new Probe(new UnicastHandler(config.MulticastAddress, config.UnicastPort), config, new Analyzer(), new ServiceHandler());
probe.StartServiceMonitoring();
>>>>>>> b197f30ffa71587a497af7041624c3d4ea9ea9af

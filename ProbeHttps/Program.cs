using ProbeHttps.analyzer;
using ProbeHttps.Config;
using ProbeHttps.Https;

// var services = new List<Service>
// {
//     new("swilabus.com", "https://www.swilabus.com/", 200, 1500),
//     new("swilabus.be", "https://www.swilabus.be/", 307, 2000),
//     new("trkr1", "https://www.swilabus.com/trkr1", 402, 1700),
//     new("trkr2", "https://www.swilabus.com/trkr2", 505, 1800)
// };
//
//
// foreach (var service in services)
// {
//     await probe.CheckService(service.Url, service.ExpectedStatusCode, service.MaxResponseTime);
// }

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

var probe = new Probe(new UnicastService(config.MulticastAddress, config.UnicastPort), config, new Analyzer());

// var multicastService = new MulticastService(config.MulticastAddress, config.MulticastPort, config.UnicastPort, config.MulticastDelay);
// multicastService.Start();


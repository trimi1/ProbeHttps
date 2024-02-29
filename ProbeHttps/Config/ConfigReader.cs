using Newtonsoft.Json;

namespace ProbeHttps.Config;

public class ConfigReader
{
    public static Configuration? GetConfig(string path)
    {
        using StreamReader reader = new(path);
        var json = reader.ReadToEnd();
        return JsonConvert.DeserializeObject<Configuration>(json);
    }
}
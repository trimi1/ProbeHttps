using Newtonsoft.Json;

namespace ProbeHttps;

public class ConfigReader
{
    public static Config? GetConfig(string path)
    {
        using StreamReader reader = new(path);
        var json = reader.ReadToEnd();
        return JsonConvert.DeserializeObject<Config>(json);
    }
}
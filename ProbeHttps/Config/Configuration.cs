namespace ProbeHttps.Config;

public record Configuration(
    string Protocol,
    string MulticastAddress,
    int MulticastPort,
    string MulticastInterface,
    int MulticastDelay,
    int UnicastPort,
    string AesKey);
namespace ProbeHttps;

public record Config(
    string Protocol,
    string MulticastAddress,
    int MulticastPort,
    string MulticastInterface,
    int MulticastDelay,
    int UnicastPort,
    string AesKey);
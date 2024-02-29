namespace ProbeHttps.Interface;

public interface IAnalyzer
{
    int Analyze(string toAnalyze);
    object? HandleSetupMessage(string message);
    object? HandleStatusOfMessage(string message);
    object? HandleStatusMessage(string message);
}
namespace ProbeHttps.Interface;

public interface IAnalyzer
{
    int Analyze(string toAnalyze);
    (int Frequency, IList<string> Aurls)? HandleSetupMessage(string message);
    int? HandleStatusOfMessage(string message);
    (int Id, string State)? HandleStatusMessage(string message);
}
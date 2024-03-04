using System.Text.RegularExpressions;
using ProbeHttps.Config;

namespace ProbeHttps.Singleton;

public class Analyzer
{
    private static Analyzer? _instance;

    private readonly IEnumerable<string> _regexList = new List<string>
    {
        Constants.SetupRegex,
        Constants.StatusOfRegex,
        Constants.StatusRegex,
    };

    public static Analyzer GetInstance()
        => _instance ??= new Analyzer();

    public int Analyze(string toAnalyze)
    {
        foreach (var r in _regexList)
        {
            if (Regex.IsMatch(toAnalyze, r))
            {
                return _regexList.Select((value, i) => new { i, value }).First(x => x.value == r).i;
            }
        }

        return -1;
    }
}
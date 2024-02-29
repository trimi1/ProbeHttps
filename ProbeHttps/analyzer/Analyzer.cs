using System.Text.RegularExpressions;
using ProbeHttps.Config;
using ProbeHttps.Interface;

namespace ProbeHttps.analyzer;

public class Analyzer : IAnalyzer
{
    private readonly IEnumerable<string> _regexList = new List<string>
    {
        Constants.SetupRegex,
        Constants.StatusOfRegex,
        Constants.StatusRegex,
    };

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
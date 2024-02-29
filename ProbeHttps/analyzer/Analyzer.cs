using System.Text.RegularExpressions;
using ProbeHttps.Interface;

namespace ProbeHttps.analyzer;

public class Analyzer : IAnalyzer
{
    private const string LetterRegex = "[A-Za-z]";
    private const string DigitRegex = "[0-9]";
    private const string LetterDigitRegex = $"({LetterRegex}|{DigitRegex})";
    private const string CrlfRegex = "\r\n";
    private const string PortRegex = $"{DigitRegex}{{1,5}}";
    private const string CharacterSpecRegex = "[-_=./+*$°()\\[\\]{}^]";
    private const string CharacterPassRegex = $"({LetterDigitRegex}|{CharacterSpecRegex})";
    private const string SpRegex = "\\x20";
    private const string IdRegex = $"{LetterDigitRegex}{{5,10}}";
    private const string ProtocolRegex = $"{LetterDigitRegex}{{3,15}}";
    private const string UsernameRegex = $"{LetterDigitRegex}{{3,50}}";
    private const string PasswordRegex = $"{CharacterPassRegex}{{3,50}}";
    private const string AuthenticationRegex = $"{CharacterPassRegex}{{3,50}}";
    private const string PasswordAuthRegex = $"{PasswordRegex}(#{AuthenticationRegex})?";
    private const string HostRegex = $"({LetterDigitRegex}|\\.|_|-){{3,50}}";
    private const string PathRegex = $"/({LetterDigitRegex}|[.\\-_]){{0,100}}";

    private const string UrlRegex =
        $"{ProtocolRegex}://({UsernameRegex}(:{PasswordAuthRegex})?@)?{HostRegex}({PortRegex})?{PathRegex}";

    private const string MinRegex = $"{DigitRegex}{{1,8}}";
    private const string MaxRegex = $"{DigitRegex}{{1,8}}";
    private const string FrequencyRegex = $"{DigitRegex}{{1,8}}";
    private const string AugmentedUrlRegex = $"{IdRegex}!{UrlRegex}!{MinRegex}!{MaxRegex}";
    private const string StateRegex = "OK|ALARM|DOWN|UNKNOWN";


    private const string SetupRegex =
        $"SETUP{SpRegex}(?<Frequency>{FrequencyRegex})(({SpRegex}(?<Aurls>{AugmentedUrlRegex})){{0,100}}){CrlfRegex}";

    private const string StatusOfRegex = $"STATUSOF{SpRegex}(?<Id>{IdRegex}){CrlfRegex}";

    private const string StatusRegex =
        $"STATUS{SpRegex}(?<Id>{IdRegex}){SpRegex}(?<State>{StateRegex}){CrlfRegex}";

    private readonly IEnumerable<string> _regexList = new List<string>
    {
        SetupRegex,
        StatusOfRegex,
        StatusRegex,
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

    public (int Frequency, IList<string> Aurls)? HandleSetupMessage(string message)
    {
        var match = Regex.Match(message, SetupRegex);
        if (!match.Success) return null;

        var frequency = match.Groups["Frequency"].Value;
        var aurs = match.Groups["Aurls"].Captures.Select(c => c.Value).ToList();

        return (Frequency: int.Parse(frequency), Aurls: aurs);
    }

    public int? HandleStatusOfMessage(string message)
    {
        var match = Regex.Match(message, StatusOfRegex);
        if (!match.Success) return null;

        return int.Parse(match.Groups["Id"].Value);
    }

    public (int Id, string State)? HandleStatusMessage(string message)
    {
        var match = Regex.Match(message, StatusRegex);
        if (!match.Success) return null;

        return (Id: int.Parse(match.Groups["Id"].Value), State: match.Groups["State"].Value);
    }
}
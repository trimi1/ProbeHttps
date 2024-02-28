using System.Text.RegularExpressions;

namespace ProbeHttps.analyzer;

public class Analyzer
{
    public const string LetterRegex = "[A-Za-z]";
    public const string DigitRegex = "[0-9]";
    public static readonly string LetterDigitRegex = "(" + LetterRegex + "|" + DigitRegex + ")";
    public const string CrlfRegex = "\r\n";
    public static readonly string PortRegex = DigitRegex + "{1,5}";
    public const string CharacterSpecRegex = "[-_=./+*$°()\\[\\]{}^]";
    public static readonly string CharacterPassRegex = "(" + LetterDigitRegex + "|" + CharacterSpecRegex + ")";
    public const string SpRegex = "\\x20";
    public static readonly string IdRegex = LetterDigitRegex + "{5,10}";
    public static readonly string ProtocolRegex = LetterDigitRegex + "{3,15}";
    public static readonly string UsernameRegex = LetterDigitRegex + "{3,50}";
    public static readonly string PasswordRegex = CharacterPassRegex + "{3,50}";
    public static readonly string AuthenticationRegex = CharacterPassRegex + "{3,50}";
    public static readonly string PasswordAuthRegex = PasswordRegex + "(#" + AuthenticationRegex + ")?";
    public static readonly string HostRegex = "(" + LetterDigitRegex + "|" + "\\." + "|" + "_" + "|" + "-" + ")" + "{3,50}";
    public static readonly string PathRegex = "/(" + LetterDigitRegex + "|[.\\-_]){0,100}";
    public static readonly string UrlRegex = ProtocolRegex + "://(" + UsernameRegex + "(:"
                                              + PasswordAuthRegex + ")?@)?" + HostRegex + "(" + PortRegex + ")?" + PathRegex;
    public static readonly string MinRegex = DigitRegex + "{1,8}";
    public static readonly string MaxRegex = DigitRegex + "{1,8}";
    public static readonly string FrequencyRegex = DigitRegex + "{1,8}";
    public static readonly string AugmentedUrlRegex = IdRegex + "!" + UrlRegex + "!" + MinRegex + "!" + MaxRegex;
    public const string StateRegex = "OK|ALARM|DOWN|UNKNOWN";


    public static string SetupRegex = "SETUP" + SpRegex + FrequencyRegex + "((" + SpRegex + AugmentedUrlRegex + "){0,100})" + CrlfRegex;
    public static string StatusofRegex = "STATUSOF" + SpRegex + IdRegex + CrlfRegex;
    public static string StatusRegex = "STATUS" + SpRegex + IdRegex + SpRegex + StateRegex + CrlfRegex;

    private IEnumerable<string> _regexList = new List<string>()
    {
        LetterRegex,
        DigitRegex,
        LetterDigitRegex,
        CrlfRegex,
        PortRegex,
        CharacterSpecRegex,
        CharacterPassRegex,
        SpRegex,
        IdRegex,
        ProtocolRegex,
        UsernameRegex,
        PasswordRegex,
        AuthenticationRegex,
        PasswordAuthRegex,
        HostRegex,
        PathRegex,
        UrlRegex,
        MinRegex,
        MaxRegex,
        FrequencyRegex,
        AugmentedUrlRegex,
        StateRegex,
        SetupRegex,
        StatusofRegex,
        StatusRegex,
    };

    public int Analyze(string toAnalyze)
    {
        foreach (var r in _regexList)
        {
            if (Regex.IsMatch(toAnalyze, r))
            {
                return _regexList.Select((value,i)=>new {i,value}).First(x=>x.value==r).i;
            }
        }
        return -1;
    }

}
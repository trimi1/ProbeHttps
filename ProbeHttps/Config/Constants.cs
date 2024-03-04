namespace ProbeHttps.Config;

public static class Constants
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

    public const string SetupRegex =
        $"SETUP{SpRegex}(?<Frequency>{FrequencyRegex})(({SpRegex}(?<Aurls>{AugmentedUrlRegex})){{0,100}}){CrlfRegex}";

    public const string StatusOfRegex = $"STATUSOF{SpRegex}(?<Id>{IdRegex}){CrlfRegex}";

    public const string StatusRegex =
        $"STATUS{SpRegex}(?<Id>{IdRegex}){SpRegex}(?<State>{StateRegex}){CrlfRegex}";
    
    public const string Protocol = "https";
}
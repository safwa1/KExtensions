using System.Globalization;

namespace KExtensions.Types;

public readonly struct InvalidNameMessage(string? arMessage, string? enMessage)
{
    private string ArMessage { get; } = arMessage ?? string.Empty;
    private string EnMessage { get; } = enMessage ?? string.Empty;

    

    public string GetMessage(CultureInfo? culture = null)
    {
        culture ??= CultureInfo.CurrentCulture;
        
        if (culture.TwoLetterISOLanguageName.Equals("ar", StringComparison.OrdinalIgnoreCase))
            return ArMessage;
        
        return EnMessage;
    }

    public override string ToString() => GetMessage();
}

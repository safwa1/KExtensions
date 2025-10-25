using System.Net;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace KExtensions;

public static partial class CheckStringsExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsUuid(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return Guid.TryParse(value, out _);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsGuid(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return Guid.TryParse(value, out _);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsMacAddress(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return MacAddressRegex().IsMatch(value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsIpv4(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return Ipv4Regex().IsMatch(value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsIpv6(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return Ipv6Regex().IsMatch(value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsIpAddress(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return IPAddress.TryParse(value, out _);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEmail(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return EmailRegex().IsMatch(value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsHexColor(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return HexColorRegex().IsMatch(value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsDate(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return DateTime.TryParse(value, out _);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsTime(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return TimeSpan.TryParse(value, out _);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsUrl(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return UrlRegex().IsMatch(value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsCreditCard(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (!CreditCardRegex().IsMatch(value)) return false;
        return LuhnCheck(value);
    }

    private static bool LuhnCheck(string digits)
    {
        int sum = 0;
        bool alt = false;
        for (int i = digits.Length - 1; i >= 0; i--)
        {
            int n = digits[i] - '0';
            if (n < 0 || n > 9) return false;
            if (alt)
            {
                n *= 2;
                if (n > 9) n -= 9;
            }
            sum += n;
            alt = !alt;
        }
        return sum % 10 == 0;
    }

    [GeneratedRegex(@"^(http|https|ftp)://[^\s/$.?#].[^\s]*$")]
    private static partial Regex UrlRegex();

    [GeneratedRegex(@"^([0-9A-Fa-f]{2}[:-]){5}([0-9A-Fa-f]{2})$")]
    private static partial Regex MacAddressRegex();

    [GeneratedRegex(@"^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$")]
    private static partial Regex Ipv4Regex();

    [GeneratedRegex(@"^(([0-9a-fA-F]{1,4}:){7,7}[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,7}:|([0-9a-fA-F]{1,4}:){1,6}:[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,5}(:[0-9a-fA-F]{1,4}){1,2}|([0-9a-fA-F]{1,4}:){1,4}(:[0-9a-fA-F]{1,4}){1,3}|([0-9a-fA-F]{1,4}:){1,3}(:[0-9a-fA-F]{1,4}){1,4}|([0-9a-fA-F]{1,4}:){1,2}(:[0-9a-fA-F]{1,4}){1,5}|[0-9a-fA-F]{1,4}:((:[0-9a-fA-F]{1,4}){1,6})|:((:[0-9a-fA-F]{1,4}){1,7}|:)|fe80:(:[0-9a-fA-F]{0,4}){0,4}%[0-9A-Za-z]{1,}|::(ffff(:0{1,4}){0,1}:){0,1}((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])|([0-9a-fA-F]{1,4}:){1,4}:((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9]))$")]
    private static partial Regex Ipv6Regex();

    [GeneratedRegex(@"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$")]
    private static partial Regex HexColorRegex();

    [GeneratedRegex(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$")]
    private static partial Regex EmailRegex();

    [GeneratedRegex(@"^(?:4[0-9]{12}(?:[0-9]{3})?|5[1-5][0-9]{14}|6(?:011|5[0-9][0-9])[0-9]{12}|3[47][0-9]{13}|3(?:0[0-5]|[68][0-9])[0-9]{11}|(?:2131|1800|35\d{3})\d{11})$")]
    private static partial Regex CreditCardRegex();
}

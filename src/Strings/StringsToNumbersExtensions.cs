using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace KExtensions.Strings;

public static class StringsToNumbersExtensions
{
    /// <summary>
    /// Parses currency with symbols and thousands separators.
    /// Always uses invariant culture.
    /// Throws FormatException on invalid input.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static decimal ParseCurrency(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (decimal.TryParse(value,
                NumberStyles.Currency,
                CultureInfo.InvariantCulture,
                out var result))
            return result;

        throw new FormatException("Invalid currency format.");
    }

    /// <summary>
    /// Normalizes formatted currency strings.
    /// Removes non-numeric characters then parses.
    /// Throws FormatException on invalid input.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static decimal ParseCurrencyLoose(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        string cleaned = Regex.Replace(value, @"[^\d.,-]", "");

        // unify decimal separator
        cleaned = cleaned.Replace(",", ".");

        if (decimal.TryParse(cleaned,
                NumberStyles.Float,
                CultureInfo.InvariantCulture,
                out var result))
            return result;

        throw new FormatException("Invalid numeric currency.");
    }

    /// <summary>
    /// Parse arbitrary numeric string using invariant culture.
    /// Throws FormatException on invalid input.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static decimal ParseDecimalInvariant(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        value = value.Replace(",", ".");
        return decimal.Parse(value, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Safe parse. Returns null if parsing fails.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static decimal? TryDecimalInvariant(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        value = value.Replace(",", ".");
        return decimal.TryParse(value,
            NumberStyles.Any,
            CultureInfo.InvariantCulture,
            out var result)
            ? result
            : null;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static decimal ToDecimal(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (decimal.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var result))
            return result;
        throw new FormatException("Invalid decimal format.");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ToInt(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return int.Parse(value, CultureInfo.InvariantCulture);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long ToLong(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return long.Parse(value, CultureInfo.InvariantCulture);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double ToDouble(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return double.Parse(value, CultureInfo.InvariantCulture);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float ToFloat(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return float.Parse(value, CultureInfo.InvariantCulture);
    }
}

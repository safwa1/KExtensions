using System.Globalization;
using System.Runtime.CompilerServices;

namespace KExtensions;

public static class StringDateTimeExtensions
{
    /// <summary>
    /// Parse string to DateTime using specified date format (default: dd/MM/yyyy)
    /// Throws FormatException on invalid input.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime ParseDate(this string value, string format = "dd/MM/yyyy")
    {
        ArgumentNullException.ThrowIfNull(value);
        return DateTime.ParseExact(value, format, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Parse string to DateTime using a specified time format (default: HH:mm:ss)
    /// Returns 00:00:00 if input is null or empty.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime ParseTime(this string value, string format = "HH:mm:ss")
    {
        if (string.IsNullOrWhiteSpace(value))
            value = "00:00:00";

        return DateTime.ParseExact(value, format, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Parse string to full DateTime using format "dd/MM/yyyy HH:mm:ss"
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime ParseDateTime(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return DateTime.ParseExact(value, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Parse string to DateTime using any specified format with invariant culture
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime ParseExactInvariant(this string value, string format)
    {
        ArgumentNullException.ThrowIfNull(value);
        return DateTime.ParseExact(value, format, CultureInfo.InvariantCulture);
    }
}
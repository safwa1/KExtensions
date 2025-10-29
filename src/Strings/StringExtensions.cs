using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace KExtensions.Strings;

public static partial class StringExtensions
{
    /// <summary>
    /// Returns true if this not nullable string is empty (contains no characters).
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEmpty(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return value.Length == 0;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNullOrWhiteSpace(this string? value) => string.IsNullOrWhiteSpace(value);

    /// <summary>
    /// Returns `true` if this not nullable string is not empty.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNotEmpty(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return value.Length > 0;
    }


    /// <summary>
    /// Returns `true` if this nullable string is either `null` or empty.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNullOrEmpty(this string? value)
    {
        return string.IsNullOrEmpty(value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNotNullOrEmpty(this string? value) => !value.IsNullOrEmpty();

    /// <summary>
    /// Returns true if this char sequence is empty or consists solely of whitespace characters according to not nullable string.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsBlank(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (value.Length == 0) return true;

        for (int i = 0; i < value.Length; i++)
        {
            if (!char.IsWhiteSpace(value[i]))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Returns true if this nullable string? is either null or empty or consists solely of whitespace characters.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNullOrBlank(this string? value)
    {
        if (value == null)
            return true;
        for (int i = 0; i < value.Length; ++i)
        {
            if (!char.IsWhiteSpace(value[i]))
                return false;
        }

        return true;
    }

    /// <summary>
    /// Returns true if this not nullable string is not empty and contains some characters except whitespace characters.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNotBlank(this string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return !value.IsBlank();
    }

    /// <summary>
    /// Returns true if this nullable string? is not empty and contains some characters except whitespace characters.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNotNullOrBlank(this string? value) => !value.IsNullOrBlank();

    /// <summary>
    /// Returns the provided string value if it is not null; otherwise, returns an empty string.
    /// </summary>
    /// <param name="value">The string value to check for null.</param>
    /// <returns>The original string if not null, or an empty string if the value is null.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string OrEmpty(this string? value) => value ?? string.Empty;

    /// <summary>
    /// Returns the original string if it is not null; otherwise, returns the provided alternative value.
    /// </summary>
    /// <param name="value">The input nullable string to be checked for null.</param>
    /// <param name="alternativeValue">The alternative string to return if the input string is null.</param>
    /// <returns>The input string if not null; otherwise, the alternative string.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Or(this string? value, string alternativeValue) => value ?? alternativeValue;

    /// <summary>
    /// Returns this not nullable if it is not empty and doesn't consist solely of whitespace characters, or the result of calling [defaultValue] function otherwise.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string IfBlank(this string value, Func<string> defaultValue)
    {
        ArgumentNullException.ThrowIfNull(value);
        return value.IsBlank() ? defaultValue.Invoke() : value;
    }

    /// <summary>
    ///  * Returns this not nullable if it's not empty or the result of calling [defaultValue] function if the char sequence is empty.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string IfEmpty(this string value, Func<string> defaultValue)
    {
        ArgumentNullException.ThrowIfNull(value);
        return value.Length == 0 ? defaultValue() : value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNull(this string? args) => args is null;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNotNull(this string? args) => args is not null;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEmptyOrEqual(this string args, string value) => args.IsEmpty() || args == value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEmptyOrContains(this string args, string value) => args.IsEmpty() || args.Contains(value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int CodePointAt(this string value, int index)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (index < 0 || index > value.Length - 1) throw new ArgumentOutOfRangeException(nameof(index));
        return value[index];
    }

    // Extension method to repeat the string a specified number of times
    /// <summary>
    /// Efficiently repeats the given string <paramref name="count"/> times.
    /// Uses stackalloc for small strings to avoid heap allocations.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Repeat(this string str, int count)
    {
        ArgumentNullException.ThrowIfNull(str);
        if (count <= 0) return string.Empty;
        if (count == 1) return str;

        int len = str.Length;
        if (len == 0) return string.Empty;

        // Total output length
        int totalLen = len * count;

        // SAFE stackalloc fallback threshold
        const int stackLimit = 1024;

        Span<char> buffer = totalLen <= stackLimit
            ? stackalloc char[totalLen]
            : new char[totalLen]; // fallback to heap if too large

        // Copy the string repeatedly
        var src = str.AsSpan();
        int pos = 0;

        for (int i = 0; i < count; i++)
        {
            src.CopyTo(buffer.Slice(pos, len));
            pos += len;
        }

        return new string(buffer);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Remove(this string str, string substring, bool ignoreCase = false)
    {
        if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(substring))
            return str;

        if (!ignoreCase)
            return str.Replace(substring, string.Empty);

        int index = 0;
        int subLen = substring.Length;
        Span<char> buffer = stackalloc char[str.Length];
        int pos = 0;

        while (index < str.Length)
        {
            if (index <= str.Length - subLen &&
                str.AsSpan(index, subLen).Equals(substring.AsSpan(), StringComparison.OrdinalIgnoreCase))
            {
                index += subLen; // skip substring
            }
            else
            {
                buffer[pos++] = str[index++];
            }
        }

        return new string(buffer[..pos]);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Join(this string[]? strings, string separator)
    {
        if (strings == null || strings.Length == 0)
            return string.Empty;

        return string.Join(separator, strings);
    }

    /// <summary>
    /// Returns a subsequence of this string with the first n characters removed.
    /// </summary>
    /// <param name="str">The input string.</param>
    /// <param name="n">The number of characters to remove from the start.</param>
    /// <returns>A new string with the first n characters removed.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Drop(this string str, int n)
    {
        ArgumentNullException.ThrowIfNull(str);

        var size = str.Length - 1;

        if (n <= 0)
        {
            throw new IndexOutOfRangeException(
                $"Requested character count {n} is less than zero."
            );
        }

        if (n > size)
        {
            throw new IndexOutOfRangeException(
                $"Requested character count {n} is more than length."
            );
        }

        return str.Substring(n);
    }


    /// <summary>
    /// Returns a subsequence of this string with the last n characters removed.
    /// </summary>
    /// <param name="str">The input string.</param>
    /// <param name="n">The number of characters to remove from the end.</param>
    /// <returns>A new string with the last n characters removed.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string DropLast(this string str, int n)
    {
        ArgumentNullException.ThrowIfNull(str);
        var size = str.Length - 1;

        if (n <= 0)
        {
            throw new IndexOutOfRangeException(
                $"Requested character count {n} is less than zero."
            );
        }

        if (n > size)
        {
            throw new IndexOutOfRangeException(
                $"Requested character count {n} is more than length."
            );
        }

        return str.Substring(0, str.Length - n);
    }

    /// <summary>
    /// Returns a subsequence of this string containing all characters except the last characters that satisfy the given predicate.
    /// </summary>
    /// <param name="str">The input string.</param>
    /// <param name="predicate">The predicate to test characters.</param>
    /// <returns>A new string with the last characters satisfying the predicate removed.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string DropLastWhile(this string str, Predicate<char> predicate)
    {
        ArgumentNullException.ThrowIfNull(str);
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));

        int end = str.Length;
        while (end > 0 && predicate(str[end - 1]))
        {
            end--;
        }

        return str.Substring(0, end);
    }

    /// <summary>
    /// Returns a subsequence of this string containing all characters except the first characters that satisfy the given predicate.
    /// </summary>
    /// <param name="str">The input string.</param>
    /// <param name="predicate">The predicate to test characters.</param>
    /// <returns>A new string with the first characters satisfying the predicate removed.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string DropWhile(this string str, Predicate<char> predicate)
    {
        ArgumentNullException.ThrowIfNull(str);
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));

        int start = 0;
        while (start < str.Length && predicate(str[start]))
        {
            start++;
        }

        return str.Substring(start);
    }

    /// <summary>
    /// Returns a span that skips the first <paramref name="count"/> characters.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<char> DropSpan(this string str, int count)
    {
        ArgumentNullException.ThrowIfNull(str);
        if ((uint)count >= (uint)str.Length) // safe unsigned compare
            return ReadOnlySpan<char>.Empty;

        return str.AsSpan(count);
    }

    /// <summary>
    /// Returns a span that skips the last <paramref name="count"/> characters.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<char> DropLastSpan(this string str, int count)
    {
        ArgumentNullException.ThrowIfNull(str);
        if ((uint)count >= (uint)str.Length)
            return ReadOnlySpan<char>.Empty;

        return str.AsSpan(0, str.Length - count);
    }

    /// <summary>
    /// Returns a span excluding all trailing characters that satisfy <paramref name="predicate"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<char> DropLastWhileSpan(this string str, Func<char, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(str);
        ArgumentNullException.ThrowIfNull(predicate);

        ReadOnlySpan<char> span = str.AsSpan();
        int end = span.Length;
        while (end > 0 && predicate(span[end - 1])) end--;
        return span.Slice(0, end);
    }

    /// <summary>
    /// Returns a span excluding all leading characters that satisfy <paramref name="predicate"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<char> DropWhileSpan(this string str, Func<char, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(str);
        ArgumentNullException.ThrowIfNull(predicate);

        ReadOnlySpan<char> span = str.AsSpan();
        int start = 0;
        while (start < span.Length && predicate(span[start])) start++;
        return span.Slice(start);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNotEmptyOrEqual(this string args, string value)
    {
        return !args.IsEmpty() || args == value;
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ExtractDigits(this string s)
    {
        if (string.IsNullOrEmpty(s)) return string.Empty;
        var sb = new StringBuilder(s.Length);
        foreach (var c in s)
            if (char.IsDigit(c))
                sb.Append(c);
        return sb.ToString();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToTitleCase(this string s)
    {
        if (string.IsNullOrEmpty(s)) return string.Empty;
        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s.ToLowerInvariant());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string InsertAfter(this string s, string after, string append)
    {
        if (string.IsNullOrEmpty(s) || string.IsNullOrEmpty(after)) return s;
        int index = s.IndexOf(after, StringComparison.Ordinal);
        return index < 0 ? s : s.Insert(index + after.Length, append);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string InsertBefore(this string s, string before, string append)
    {
        if (string.IsNullOrEmpty(s) || string.IsNullOrEmpty(before)) return s;
        int index = s.IndexOf(before, StringComparison.Ordinal);
        return index < 0 ? s : s.Insert(index, append);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string InsertAt(this string s, int index, string value)
    {
        if (string.IsNullOrEmpty(s)) return value;
        if (index < 0 || index > s.Length) throw new ArgumentOutOfRangeException(nameof(index));
        return s.Insert(index, value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNumeric(this string s)
    {
        if (string.IsNullOrEmpty(s)) return false;
        foreach (var c in s)
            if (!char.IsDigit(c))
                return false;
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasDigits(this string s)
    {
        if (string.IsNullOrEmpty(s)) return false;
        foreach (var c in s)
            if (char.IsDigit(c))
                return true;
        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasLetters(this string s)
    {
        if (string.IsNullOrEmpty(s)) return false;
        foreach (var c in s)
            if (char.IsLetter(c))
                return true;
        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasLettersOrDigits(this string s)
    {
        if (string.IsNullOrEmpty(s)) return false;
        foreach (var c in s)
            if (char.IsLetterOrDigit(c))
                return true;
        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double MatchSimilarity(this string a, string b)
    {
        if (string.IsNullOrEmpty(a) || string.IsNullOrEmpty(b)) return 0d;

        int matchCount = 0;
        int minLen = Math.Min(a.Length, b.Length);
        for (int i = 0; i < minLen; i++)
            if (a[i] == b[i])
                matchCount++;

        return (matchCount / (double)a.Length) * 100.0;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string RemoveNonDigits(this string s)
    {
        if (string.IsNullOrEmpty(s)) return string.Empty;

        int len = s.Length;
        Span<char> buffer = len <= 1024
            ? stackalloc char[len]
            : new char[len];

        int count = 0;
        foreach (char c in s)
            if (char.IsDigit(c))
                buffer[count++] = c;

        return new string(buffer[..count]);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string RemoveNonAlphanumeric(this string s)
    {
        if (string.IsNullOrEmpty(s)) return string.Empty;
        int len = s.Length;
        Span<char> buffer = len <= 1024
            ? stackalloc char[len]
            : new char[len];

        int count = 0;
        foreach (char c in s)
            if (char.IsLetterOrDigit(c))
                buffer[count++] = c;

        return new string(buffer[..count]);
    }

    /// <summary>
    /// Returns a substring using flexible start and end indices (supports negative values like Python).
    /// </summary>
    /// <param name="str">The input string.</param>
    /// <param name="start">Start index (can be negative to count from the end).</param>
    /// <param name="end">End index (can be negative or omitted to go to the end).</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Slice(this string str, int start, int? end = null)
    {
        ArgumentNullException.ThrowIfNull(str);

        int len = str.Length;

        // Normalize indices (handle negatives)
        if (start < 0)
            start = len + start;
        if (end is null)
            end = len;
        else if (end < 0)
            end = len + end.Value;

        // Clamp to valid range
        start = Math.Clamp(start, 0, len);
        end = Math.Clamp(end.Value, 0, len);

        // If invalid range, return empty string
        if (end <= start)
            return string.Empty;

        int sliceLength = end.Value - start;
        return str.Substring(start, sliceLength);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Interpolate(this string str, object? data)
    {
        if (string.IsNullOrEmpty(str) || data == null) return str;

        var regex = PlaceholderRegex();
        return regex.Replace(str, match =>
        {
            var propName = match.Groups[1].Value;
            var prop = data.GetType().GetProperty(propName);
            return prop?.GetValue(data)?.ToString() ?? match.Value;
        });
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string FormatArgs(this string str, object? data)
    {
        if (string.IsNullOrEmpty(str) || data == null) return str;

        var regex = PercentPlaceholderRegex();
        return regex.Replace(str, match =>
        {
            var propName = match.Groups[1].Value;
            var prop = data.GetType().GetProperty(propName);
            return prop?.GetValue(data)?.ToString() ?? match.Value;
        });
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEnglishText(this string input, bool includePunctuation = true)
    {
        if (string.IsNullOrEmpty(input)) return true;
        foreach (var c in input)
        {
            if (char.IsLetterOrDigit(c) || char.IsWhiteSpace(c))
                continue;

            if (includePunctuation && char.IsPunctuation(c))
                continue;

            return false;
        }

        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToTrimmedString(this object? value)
    {
        return value?.ToString()?.Trim() ?? string.Empty;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Take(this string str, int start, int length)
    {
        ArgumentNullException.ThrowIfNull(str);

        if (start < 0 || length < 0)
            throw new ArgumentOutOfRangeException(null, "start and length must be non-negative.");

        if (start >= str.Length)
            return string.Empty;

        // Clamp length if it exceeds string length
        length = Math.Min(length, str.Length - start);

        return str.AsSpan(start, length).ToString();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Take(this string str, int count)
    {
        return string.IsNullOrEmpty(str) || count <= 0
            ? string.Empty
            : str.AsSpan(0, Math.Min(count, str.Length)).ToString();
    }
    

    public static double StringMatchPercentage(this string s1, string s2)
    {
        if (string.IsNullOrEmpty(s1) || string.IsNullOrEmpty(s2))
            return 0.0;

        int len1 = s1.Length;
        int len2 = s2.Length;

        var distance = new int[len1 + 1, len2 + 1];

        for (int i = 0; i <= len1; i++) distance[i, 0] = i;
        for (int j = 0; j <= len2; j++) distance[0, j] = j;

        for (int i = 1; i <= len1; i++)
        {
            for (int j = 1; j <= len2; j++)
            {
                int cost = s1[i - 1] == s2[j - 1] ? 0 : 1;
                distance[i, j] = Math.Min(
                    Math.Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1),
                    distance[i - 1, j - 1] + cost
                );
            }
        }

        int maxLen = Math.Max(len1, len2);
        int editDistance = distance[len1, len2];
        double similarity = ((maxLen - editDistance) / (double)maxLen) * 100.0;
        return Math.Max(similarity, 0.0);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Range Indices(this string str, bool inclusive = false)
    {
        ArgumentNullException.ThrowIfNull(str);

        if (!inclusive)
            return ..str.Length;

        // inclusive = true
        return str.Length > 0 ? ..(str.Length - 1) : ..0;
    }
    
    /// <summary>
    /// Splits the string into a pair, 
    /// first = chars for which predicate returns true,
    /// second = chars for which predicate returns false.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (string first, string second) Partition(this string str, Func<char, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(str);
        ArgumentNullException.ThrowIfNull(predicate);

        int len = str.Length;
        if (len == 0) return (string.Empty, string.Empty);

        const int stackLimit = 1024;

        Span<char> trueBuffer = len <= stackLimit ? stackalloc char[len] : new char[len];
        Span<char> falseBuffer = len <= stackLimit ? stackalloc char[len] : new char[len];

        int tCount = 0;
        int fCount = 0;

        foreach (char c in str)
        {
            if (predicate(c))
                trueBuffer[tCount++] = c;
            else
                falseBuffer[fCount++] = c;
        }

        return (new string(trueBuffer[..tCount]), new string(falseBuffer[..fCount]));
    }

    /// <summary>
    /// Counts the number of words in a string.
    /// Words are separated by spaces, dots, question marks, or tabs/newlines.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int WordCount(this string str)
    {
        if (string.IsNullOrWhiteSpace(str))
            return 0;

        ReadOnlySpan<char> span = str.AsSpan();
        int count = 0;
        bool inWord = false;

        foreach (char c in span)
        {
            if (char.IsWhiteSpace(c) || c == '.' || c == '?' || c == '!' || c == ',')
            {
                if (inWord)
                {
                    count++;
                    inWord = false;
                }
            }
            else
            {
                inWord = true;
            }
        }

        if (inWord)
            count++;

        return count;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string TakeLast(this string? str, int count)
    {
        int len = str?.Length ?? 0;
        if (count <= 0 || len == 0) return string.Empty;
        return str.AsSpan(Math.Max(0, len - count)).ToString();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string TakeWhile(this string str, Func<char, bool> predicate)
    {
        if (string.IsNullOrEmpty(str)) return str;
        return TakeWhile(str.AsSpan(), predicate).ToString();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<char> TakeWhile(this ReadOnlySpan<char> span, Func<char, bool> predicate)
    {
        if (span.IsEmpty) return ReadOnlySpan<char>.Empty;
        
        int i = 0;
        while (i < span.Length && predicate(span[i])) i++;
        return span[..i];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Capitalize(this string? str)
    {
        if (string.IsNullOrEmpty(str)) return string.Empty;
        if (char.IsUpper(str[0])) return str;

        return string.Create(str.Length, str, (span, s) =>
        {
            span[0] = char.ToUpperInvariant(s[0]);
            s.AsSpan(1).CopyTo(span.Slice(1));
        });
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Decapitalize(this string? str)
    {
        if (string.IsNullOrEmpty(str)) return string.Empty;
        if (char.IsLower(str[0])) return str;

        return string.Create(str.Length, str, (span, s) =>
        {
            span[0] = char.ToLowerInvariant(s[0]);
            s.AsSpan(1).CopyTo(span.Slice(1));
        });
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Map(this string? str, Func<char, char> transform)
    {
        if (string.IsNullOrEmpty(str)) return string.Empty;
    
        return string.Create(str!.Length, str, (span, s) =>
        {
            for (int i = 0; i < s.Length; i++)
                span[i] = transform(s[i]);
        });
    }
    
    /// <summary>
    /// Converts a string into a byte array using the specified encoding (default UTF-8).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte[] ToByteArray(this string? value, Encoding? encoding = null)
    {
        if (string.IsNullOrEmpty(value))
            return [];

        encoding ??= Encoding.UTF8;
        return encoding.GetBytes(value);
    }

    /// <summary>
    /// Decodes a Base64-encoded string into a byte array.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte[] Base64StringToByteArray(this string? value)
    {
        if (string.IsNullOrEmpty(value))
            return [];

        return Convert.FromBase64String(value);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string SubstringAfter(this string? source, string? delimiter, string defaultValue = "")
    {
        if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(delimiter))
            return defaultValue;

        int index = source.IndexOf(delimiter, StringComparison.OrdinalIgnoreCase);
        return (index >= 0 && index + delimiter.Length < source.Length)
            ? source.AsSpan(index + delimiter.Length).ToString()
            : defaultValue;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string SubstringBefore(this string? source, string? delimiter, string defaultValue = "")
    {
        if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(delimiter))
            return defaultValue;

        int index = source.IndexOf(delimiter, StringComparison.OrdinalIgnoreCase);
        return index >= 0
            ? source.AsSpan(0, index).ToString()
            : defaultValue;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string SubstringBeforeLast(this string? source, string? delimiter, string defaultValue = "")
    {
        if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(delimiter))
            return defaultValue;

        int index = source.LastIndexOf(delimiter, StringComparison.Ordinal);
        return index >= 0
            ? source.AsSpan(0, index).ToString()
            : defaultValue;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string SubstringAfterLast(this string? source, string? delimiter, string defaultValue = "")
    {
        if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(delimiter))
            return defaultValue;

        int index = source.LastIndexOf(delimiter, StringComparison.Ordinal);
        return (index >= 0 && index + delimiter.Length < source.Length)
            ? source.AsSpan(index + delimiter.Length).ToString()
            : defaultValue;
    }
    
    /// <summary>
    /// Converts a PascalCase or camelCase string into kebab-case.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToKebabCase(this string str)
    {
        if (string.IsNullOrEmpty(str)) return string.Empty;

        var sb = new StringBuilder(str.Length + 5);
        for (int i = 0; i < str.Length; i++)
        {
            char c = str[i];
            if (char.IsUpper(c))
            {
                if (i > 0) sb.Append('-');
                sb.Append(char.ToLowerInvariant(c));
            }
            else
            {
                sb.Append(c);
            }
        }
        return sb.ToString();
    }

    /// <summary>
    /// Converts a kebab-case string into camelCase.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToCamelCase(this string value)
    {
        if (string.IsNullOrEmpty(value)) return string.Empty;

        var parts = value.Split('-', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 0) return string.Empty;

        var sb = new StringBuilder(value.Length);
        sb.Append(parts[0].ToLowerInvariant()); // first word lowercase

        for (int i = 1; i < parts.Length; i++)
        {
            var word = parts[i];
            if (!string.IsNullOrEmpty(word))
            {
                sb.Append(char.ToUpperInvariant(word[0]));
                if (word.Length > 1) sb.Append(word[1..]);
            }
        }

        return sb.ToString();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string EmptyIfNull(this string? text) => text ?? string.Empty;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string? NullIfEmpty(this string text) => string.IsNullOrEmpty(text) ? null : text;

    [GeneratedRegex(@"{(.*?)}", RegexOptions.Compiled | RegexOptions.IgnoreCase, "en-US")]
    private static partial Regex PlaceholderRegex();

    [GeneratedRegex(@"%(.*?)%", RegexOptions.Compiled | RegexOptions.IgnoreCase, "en-US")]
    private static partial Regex PercentPlaceholderRegex();
    
    
#if NET10_0_OR_GREATER
    extension(string str)
    {
        public bool IsEmpty => str.IsEmpty();

        public bool IsNotEmpty => str.IsNotEmpty();
    }

    extension(string? str)
    {
        public bool IsNullOrEmpty => str.IsNullOrEmpty();

        public bool IsNullOrWhiteSpace => str.IsNullOrWhiteSpace();

        public bool IsBlank => str != null && str.IsBlank();

        public bool IsNotBlank => str != null && str.IsNotBlank();
    }

#endif
}
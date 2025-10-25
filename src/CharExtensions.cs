using System.Globalization;

namespace KExtensions;

public static class CharExtensions
{
    /// <summary>
    /// Returns <see langword="true"/> if <paramref name="c"/> is an ASCII
    /// character ([ U+0000..U+007F ]).
    /// </summary>
    /// <remarks>
    /// Per http://www.unicode.org/glossary/#ASCII, ASCII is only U+0000..U+007F.
    /// </remarks>
    public static bool IsAscii(this char c) => char.IsAscii(c);

    /// <summary>Indicates whether a character is categorized as a lowercase ASCII letter.</summary>
    /// <param name="c">The character to evaluate.</param>
    /// <returns>true if <paramref name="c"/> is a lowercase ASCII letter; otherwise, false.</returns>
    /// <remarks>
    /// This determines whether the character is in the range 'a' through 'z', inclusive.
    /// </remarks>
    public static bool IsAsciiLetterLower(this char c) => IsBetween(c, 'a', 'z');

    /// <summary>Indicates whether a character is categorized as an uppercase ASCII letter.</summary>
    /// <param name="c">The character to evaluate.</param>
    /// <returns>true if <paramref name="c"/> is a lowercase ASCII letter; otherwise, false.</returns>
    /// <remarks>
    /// This determines whether the character is in the range 'a' through 'z', inclusive.
    /// </remarks>
    public static bool IsAsciiLetterUpper(this char c) => IsBetween(c, 'A', 'Z');

    /// <summary>Indicates whether a character is categorized as an ASCII digit.</summary>
    /// <param name="c">The character to evaluate.</param>
    /// <returns>true if <paramref name="c"/> is an ASCII digit; otherwise, false.</returns>
    /// <remarks>
    /// This determines whether the character is in the range '0' through '9', inclusive.
    /// </remarks>
    public static bool IsAsciiDigit(this char c) => IsBetween(c, '0', '9');

    /*=================================IsDigit======================================
    **A wrapper for char.  Returns a boolean indicating whether    **
    **character c is considered to be a digit.                                    **
    ==============================================================================*/
    // Determines whether a character is a digit.
    public static bool IsDigit(this char c) => char.IsDigit(c);

    /// <summary>Indicates whether a character is within the specified inclusive range.</summary>
    /// <param name="c">The character to evaluate.</param>
    /// <param name="minInclusive">The lower bound, inclusive.</param>
    /// <param name="maxInclusive">The upper bound, inclusive.</param>
    /// <returns>true if <paramref name="c"/> is within the specified range; otherwise, false.</returns>
    /// <remarks>
    /// The method does not validate that <paramref name="maxInclusive"/> is greater than or equal
    /// to <paramref name="minInclusive"/>.  If <paramref name="maxInclusive"/> is less than
    /// <paramref name="minInclusive"/>, the behavior is undefined.
    /// </remarks>
    public static bool IsBetween(this char c, char minInclusive, char maxInclusive) =>
        (uint)(c - minInclusive) <= (uint)(maxInclusive - minInclusive);
    
    public static bool IsLetter(this char c) => char.IsLetter(c);
    
    public static bool IsWhiteSpace(this char c) => char.IsWhiteSpace(c);
    
    public static bool IsUpper(this char c) => char.IsUpper(c);

    public static bool IsLower(this char c) => char.IsLower(c);
    
    public static bool IsPunctuation(this char c) => char.IsPunctuation(c);

    public static bool IsLetterOrDigit(this char c) => char.IsLetterOrDigit(c);

    public static char ToUpper(char c, CultureInfo culture) => char.ToUpper(c, culture);
    
    public static char ToUpper(this char c) => char.ToUpper(c);

    public static char ToUpperInvariant(this char c) => char.ToUpperInvariant(c);

    public static char ToLower(this char c, CultureInfo culture) => char.ToLower(c, culture);
    
    public static char ToLower(this char c) => char.ToLower(c);

    public static char ToLowerInvariant(this char c) => char.ToLowerInvariant(c);

    public static bool IsControl(this char c) => char.IsControl(c);

    public static bool IsDigitAt(this string s, int index) => char.IsDigit(s, index);

    public static bool IsLetterAt(this string s, int index) => char.IsLetter(s, index);

    public static bool IsLetterOrDigitAt(this string s, int index) => char.IsLetterOrDigit(s, index);

    public static bool IsLowerAt(this string s, int index) => char.IsLower(s, index);

    public static bool IsNumber(this char c) => char.IsNumber(c);

    public static bool IsNumberAt(this string s, int index) => char.IsNumber(s, index);

    public static bool IsPunctuationAt(this string s, int index) => char.IsPunctuation(s, index);

    public static bool IsSeparator(this char c) => char.IsSeparator(c);

    public static bool IsSeparatorAt(this string s, int index) => char.IsSeparator(s, index);

    public static bool IsSurrogate(this char c) => char.IsSurrogate(c);

    public static bool IsSurrogateAt(this string s, int index) => char.IsSurrogate(s, index);

    public static bool IsSymbol(this char c)  => char.IsSymbol(c);

    public static bool IsSymbolAt(this string s, int index) => char.IsSymbol(s, index);

    public static bool IsUpperAt(this string s, int index) => char.IsUpper(s, index);

    public static bool IsWhiteSpaceAt(this string s, int index) => char.IsWhiteSpace(s, index);

    public static UnicodeCategory GetUnicodeCategory(this char c) => char.GetUnicodeCategory(c);

    public static UnicodeCategory GetUnicodeCategoryAt(this string s, int index) => char.GetUnicodeCategory(s, index);

    public static double GetNumericValue(this char c) => char.GetNumericValue(c);

    public static double GetNumericValueAt(this string s, int index) => char.GetNumericValue(s, index);
    
    public static bool IsHighSurrogate(this char c) => char.IsHighSurrogate(c);

    public static bool IsLowSurrogate(this char c) => char.IsLowSurrogate(c);

    public static bool IsLowSurrogate( this string s, int index) => char.IsLowSurrogate(s, index);
    
    public static bool IsSurrogatePairAt(this string s, int index) => char.IsSurrogatePair(s, index);

}
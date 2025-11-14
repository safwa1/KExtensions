using System.Collections;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace KExtensions.Types;

#region HijriSpan
/// <summary>
/// Represents a span in Hijri terms: days, months, years.
/// Immutable value type.
/// </summary>
public readonly struct HijriSpan : IEquatable<HijriSpan>
{
    public int Days { get; }
    public int Months { get; }
    public int Years { get; }

    public HijriSpan(int days = 0, int months = 0, int years = 0)
    {
        Days = days;
        Months = months;
        Years = years;
    }

    public static HijriSpan FromDays(int days) => new(days, 0, 0);
    public static HijriSpan FromMonths(int months) => new(0, months, 0);
    public static HijriSpan FromYears(int years) => new(0, 0, years);

    public override bool Equals(object? obj) => obj is HijriSpan s && Equals(s);
    public bool Equals(HijriSpan other) => Days == other.Days && Months == other.Months && Years == other.Years;
    public override int GetHashCode() => HashCode.Combine(Days, Months, Years);

    public static HijriSpan operator +(HijriSpan a, HijriSpan b)
        => new(a.Days + b.Days, a.Months + b.Months, a.Years + b.Years);

    public static HijriSpan operator -(HijriSpan a, HijriSpan b)
        => new(a.Days - b.Days, a.Months - b.Months, a.Years - b.Years);

    public override string ToString()
    {
        var sb = new StringBuilder();
        if (Years != 0) sb.Append(Years).Append("y ");
        if (Months != 0) sb.Append(Months).Append("m ");
        if (Days != 0) sb.Append(Days).Append("d");
        return sb.Length == 0 ? "0d" : sb.ToString().Trim();
    }
}
#endregion

#region HijriDate
public readonly struct HijriDate : IComparable<HijriDate>, IEquatable<HijriDate>, IFormattable, ISpanFormattable
{
    private static readonly Lazy<HijriCalendar> LazyCalendar = new(() =>
    {
        var cal = new HijriCalendar { HijriAdjustment = HijriDate.HijriAdjustment };
        return cal;
    });

    public static HijriCalendar Calendar => LazyCalendar.Value;
    
    public static int HijriAdjustment { get; set; } = 0;
    
    public int Year { get; }
    public int Month { get; }
    public int Day { get; }
    public int Hour { get; }
    public int Minute { get; }
    public int Second { get; }
    public int Millisecond { get; }

    public HijriDate(int year, int month, int day)
    {
        // validate using calendar (throws if invalid)
        Calendar.ToDateTime(year, month, day, 0, 0, 0, 0);
        Year = year;
        Month = month;
        Day = day;
        Hour = 0;
        Minute = 0;
        Second = 0;
        Millisecond = 0;
    }

    public HijriDate(int year, int month, int day, int hour, int minute, int second, int millisecond = 0)
    {
        Calendar.ToDateTime(year, month, day, hour, minute, second, millisecond);
        Year = year;
        Month = month;
        Day = day;
        Hour = hour;
        Minute = minute;
        Second = second;
        Millisecond = millisecond;
    }

    public static HijriDate Today
    {
        get
        {
            var now = DateTime.Now;
            return new HijriDate(
                Calendar.GetYear(now),
                Calendar.GetMonth(now),
                Calendar.GetDayOfMonth(now)
            );
        }
    }

    public static HijriDate Now
    {
        get
        {
            var now = DateTime.Now;
            return new HijriDate(
                Calendar.GetYear(now),
                Calendar.GetMonth(now),
                Calendar.GetDayOfMonth(now),
                now.Hour, now.Minute, now.Second, now.Millisecond
            );
        }
    }

    public DateTime ToDateTime()
        => Calendar.ToDateTime(Year, Month, Day, Hour, Minute, Second, Millisecond);

    public DateTime ToDateTime(int hour = 0, int minute = 0, int second = 0, int ms = 0)
        => Calendar.ToDateTime(Year, Month, Day, hour, minute, second, ms);

    public override string ToString() => $"{Year:D4}/{Month:D2}/{Day:D2}";

    // Culture-aware ToString: if current culture uses Hijri or is Arabic, present a friendly format
    public string ToString(string? format, IFormatProvider? provider)
    {
        // If no format use invariant Hijri format or localized default
        if (string.IsNullOrEmpty(format))
        {
            var culture = provider as CultureInfo ?? CultureInfo.CurrentCulture;
            if (IsCultureHijriPreferred(culture))
            {
                // e.g., "10 رمضان 1446 هـ" (day monthname year)
                return $"{Day} {GetMonthName(Month, culture.TwoLetterISOLanguageName == "ar" ? "ar" : "en")} {Year} هـ";
            }
            return ToString();
        }

        // Forward formatting to underlying DateTime with requested format - keeps standard tokens
        try
        {
            var dt = ToDateTime();
            return dt.ToString(format, provider ?? CultureInfo.CurrentCulture);
        }
        catch
        {
            return ToString();
        }
    }

    // IFormattable implementation
    string IFormattable.ToString(string? format, IFormatProvider? formatProvider) => ToString(format, formatProvider);

    // ISpanFormattable implementation (simple, delegates to string-based ToString)
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        var s = ToString(format.IsEmpty ? null : format.ToString(), provider);
        if (s.AsSpan().TryCopyTo(destination))
        {
            charsWritten = s.Length;
            return true;
        }
        charsWritten = 0;
        return false;
    }

    #region Parsing (culture-aware, arabic digits, exact)
    private static readonly char[] AllowedSeparators = { '/', '-', ' ' };
    private static readonly char[] ArabicDigits = { '٠', '١', '٢', '٣', '٤', '٥', '٦', '٧', '٨', '٩' };
    private static readonly char[] LatinDigits  = { '0','1','2','3','4','5','6','7','8','9' };

    private static string NormalizeDigits(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;
        for (int i = 0; i < ArabicDigits.Length; i++)
            input = input.Replace(ArabicDigits[i], LatinDigits[i]);
        return input;
    }

    private static bool IsCultureHijriPreferred(CultureInfo? culture)
    {
        if (culture == null) culture = CultureInfo.CurrentCulture;
        // heuristic: Arabic cultures or cultures whose Calendar is Hijri
        if (culture.Name.StartsWith("ar", StringComparison.OrdinalIgnoreCase)) return true;
        var cal = culture.DateTimeFormat.Calendar;
        return cal is HijriCalendar;
    }

    public static bool TryParseFlexible(string? value, out HijriDate result)
    {
        result = default;
        if (string.IsNullOrWhiteSpace(value)) return false;
        value = NormalizeDigits(value!).Trim();

        // Accept yyyy/MM/dd or yyyy-MM-dd or dd MMMM yyyy (with ar/en month names)
        foreach (var sep in AllowedSeparators)
        {
            var parts = value.Split(sep);
            if (parts.Length == 3 &&
                int.TryParse(parts[0], out int p0) &&
                int.TryParse(parts[1], out int p1) &&
                int.TryParse(parts[2], out int p2))
            {
                // try detect order: if year > 31 treat as yyyy/MM/dd
                if (p0 > 31)
                {
                    try { result = new HijriDate(p0, p1, p2); return true; }
                    catch { continue; }
                }
                // else maybe dd/mm/yyyy
                if (p2 > 31)
                {
                    try { result = new HijriDate(p2, p1, p0); return true; }
                    catch { continue; }
                }
            }
        }

        // try "dd MMMM yyyy" with month names
        var tokens = value.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (tokens.Length == 3)
        {
            var (t1, t2, t3) = (tokens[0], tokens[1], tokens[2]);
            if (int.TryParse(t1, out int day) && int.TryParse(NormalizeDigits(t3), out int year))
            {
                for (int m = 1; m <= 12; m++)
                {
                    if (string.Equals(GetMonthName(m, "ar"), t2, StringComparison.CurrentCultureIgnoreCase)
                        || string.Equals(GetMonthName(m, "en"), t2, StringComparison.CurrentCultureIgnoreCase))
                    {
                        try { result = new HijriDate(year, m, day); return true; }
                        catch { break; }
                    }
                }
            }
        }

        return false;
    }

    public static HijriDate ParseFlexible(string value)
        => TryParseFlexible(value, out var r) ? r : throw new FormatException("Invalid Hijri date format");

    public static bool TryParseExact(string? value, string format, out HijriDate result)
    {
        result = default;
        if (string.IsNullOrWhiteSpace(value) || string.IsNullOrWhiteSpace(format)) return false;
        value = NormalizeDigits(value!).Trim();
        format = format.Trim();

        // numeric exact formats with separators
        char? sep = null;
        foreach (var s in AllowedSeparators)
            if (format.Contains(s)) { sep = s; break; }

        if (sep != null)
        {
            var fmtParts = format.Split(sep.Value);
            var valParts = value.Split(sep.Value);
            if (fmtParts.Length == 3 && valParts.Length == 3)
            {
                int? y = null, m = null, d = null;
                for (int i = 0; i < 3; i++)
                {
                    var f = fmtParts[i].ToLowerInvariant();
                    var v = valParts[i];
                    if (!int.TryParse(v, out int num)) return false;
                    if (f.Contains("y")) y = num;
                    else if (f.Contains("m")) m = num;
                    else if (f.Contains("d")) d = num;
                }
                if (y.HasValue && m.HasValue && d.HasValue)
                {
                    try { result = new HijriDate(y.Value, m.Value, d.Value); return true; }
                    catch { return false; }
                }
            }
        }

        // fallback to flexible
        return TryParseFlexible(value, out result);
    }

    public static HijriDate ParseExact(string value, string format)
        => TryParseExact(value, format, out var r) ? r : throw new FormatException("Invalid Hijri date for given format");
    #endregion

    #region Arithmetic & Operators
    public HijriDate AddDays(int days) => FromDateTime(Calendar.AddDays(ToDateTime(), days));
    public HijriDate AddMonths(int months) => FromDateTime(Calendar.AddMonths(ToDateTime(), months));
    public HijriDate AddYears(int years) => FromDateTime(Calendar.AddYears(ToDateTime(), years));

    public static HijriDate FromDateTime(DateTime dt)
        => new(
            Calendar.GetYear(dt),
            Calendar.GetMonth(dt),
            Calendar.GetDayOfMonth(dt),
            dt.Hour, dt.Minute, dt.Second, dt.Millisecond
        );

    // operators with int (days)
    public static HijriDate operator +(HijriDate d, int days) => d.AddDays(days);
    public static HijriDate operator -(HijriDate d, int days) => d.AddDays(-days);

    // operators with HijriSpan
    public static HijriDate operator +(HijriDate d, HijriSpan span)
    {
        var dt = d.ToDateTime();
        if (span.Years != 0) dt = Calendar.AddYears(dt, span.Years);
        if (span.Months != 0) dt = Calendar.AddMonths(dt, span.Months);
        if (span.Days != 0) dt = Calendar.AddDays(dt, span.Days);
        return FromDateTime(dt);
    }

    public static HijriDate operator -(HijriDate d, HijriSpan span)
        => d + new HijriSpan(-span.Days, -span.Months, -span.Years);

    public static HijriSpan operator -(HijriDate a, HijriDate b)
    {
        // returns difference in days approximately by DateTime subtraction (miladi-based difference)
        var span = a.ToDateTime() - b.ToDateTime();
        return HijriSpan.FromDays(span.Days);
    }

    // conversions
    public static implicit operator DateTime(HijriDate h)
        => Calendar.ToDateTime(h.Year, h.Month, h.Day, h.Hour, h.Minute, h.Second, h.Millisecond);

    public static explicit operator HijriDate(DateTime dt)
        => FromDateTime(dt);
    #endregion

    #region Comparison/Equality
    public int CompareTo(HijriDate other) => ToDateTime().CompareTo(other.ToDateTime());
    public bool Equals(HijriDate other) =>
        Year == other.Year &&
        Month == other.Month &&
        Day == other.Day &&
        Hour == other.Hour &&
        Minute == other.Minute &&
        Second == other.Second &&
        Millisecond == other.Millisecond;

    public override bool Equals(object? obj) => obj is HijriDate h && Equals(h);
    public override int GetHashCode() => HashCode.Combine(Year, Month, Day, Hour, Minute, Second, Millisecond);

    public static bool operator >(HijriDate a, HijriDate b) => a.CompareTo(b) > 0;
    public static bool operator <(HijriDate a, HijriDate b) => a.CompareTo(b) < 0;
    public static bool operator >=(HijriDate a, HijriDate b) => a.CompareTo(b) >= 0;
    public static bool operator <=(HijriDate a, HijriDate b) => a.CompareTo(b) <= 0;
    #endregion

    #region Names & helpers
    private static readonly string[] MonthNamesAr =
    {
        "محرم","صفر","ربيع الأول","ربيع الآخر",
        "جمادى الأولى","جمادى الآخرة","رجب","شعبان",
        "رمضان","شوال","ذو القعدة","ذو الحجة"
    };

    private static readonly string[] MonthNamesEn =
    {
        "Muharram","Safar","Rabi' al-awwal","Rabi' al-thani",
        "Jumada al-awwal","Jumada al-thani","Rajab","Sha'ban",
        "Ramadan","Shawwal","Dhu al-Qi'dah","Dhu al-Hijjah"
    };

    public static string GetMonthName(int month, string culture = "ar")
    {
        if (month is < 1 or > 12) throw new ArgumentOutOfRangeException(nameof(month));
        return culture switch
        {
            "ar" => MonthNamesAr[month - 1],
            "en" => MonthNamesEn[month - 1],
            _ => MonthNamesAr[month - 1]
        };
    }

    public string MonthName => GetMonthName(Month);
    public string DayName => ToDateTime().ToString("dddd", CultureInfo.CurrentCulture);

    public bool IsWeekend(string? culture = null)
    {
        var c = culture == null ? CultureInfo.CurrentCulture : new CultureInfo(culture);
        var dow = ToDateTime().DayOfWeek;
        if (IsCultureHijriPreferred(c))
        {
            // typical Arabic weekend = Friday (and sometimes Thursday/Friday or Friday/Saturday depending locale)
            return dow == DayOfWeek.Friday;
        }
        // default global weekend Saturday/Sunday
        return dow is DayOfWeek.Saturday or DayOfWeek.Sunday;
    }
    #endregion

    #region Unix time helpers
    public long ToUnixTimeSeconds()
    {
        var dto = new DateTimeOffset(ToDateTime().ToUniversalTime());
        return dto.ToUnixTimeSeconds();
    }

    public static HijriDate FromUnixTimeSeconds(long seconds)
    {
        var dto = DateTimeOffset.FromUnixTimeSeconds(seconds).UtcDateTime;
        var local = dto.ToLocalTime();
        return FromDateTime(local);
    }
    #endregion
}

// JSON converter (System.Text.Json)
public class HijriDateJsonConverter : JsonConverter<HijriDate>
{
    public override HijriDate Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var s = reader.GetString() ?? throw new JsonException("Invalid Hijri date value");
        return HijriDate.ParseFlexible(s);
    }

    public override void Write(Utf8JsonWriter writer, HijriDate value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
#endregion

public readonly struct HijriRange : IEnumerable<HijriDate>
{
    public HijriDate Start { get; }
    public HijriDate End { get; }

    public HijriRange(HijriDate start, HijriDate end)
    {
        if (start.CompareTo(end) > 0) throw new ArgumentException("Start must be <= End");
        Start = start;
        End = end;
    }

    public bool Contains(HijriDate d) => d.CompareTo(Start) >= 0 && d.CompareTo(End) <= 0;

    public bool Overlaps(HijriRange other)
        => !(other.End.CompareTo(Start) < 0 || other.Start.CompareTo(End) > 0);

    public HijriRange Intersect(HijriRange other)
    {
        if (!Overlaps(other)) throw new InvalidOperationException("Ranges do not overlap");
        var s = Start.CompareTo(other.Start) >= 0 ? Start : other.Start;
        var e = End.CompareTo(other.End) <= 0 ? End : other.End;
        return new HijriRange(s, e);
    }

    public HijriRange Union(HijriRange other)
    {
        var s = Start.CompareTo(other.Start) <= 0 ? Start : other.Start;
        var e = End.CompareTo(other.End) >= 0 ? End : other.End;
        return new HijriRange(s, e);
    }

    public HijriRange ExpandByDays(int days)
        => new HijriRange(Start.AddDays(-days), End.AddDays(days));

    public HijriRange ContractByDays(int days)
    {
        var s = Start.AddDays(days);
        var e = End.AddDays(-days);
        if (s.CompareTo(e) > 0) throw new InvalidOperationException("Contracted range is empty");
        return new HijriRange(s, e);
    }

    public IEnumerable<HijriDate> GetDays()
    {
        var cur = Start;
        while (cur.CompareTo(End) <= 0)
        {
            yield return cur;
            cur = cur.AddDays(1);
        }
    }

    public int DayCount()
    {
        var span = End.ToDateTime().Date - Start.ToDateTime().Date;
        return Math.Abs(span.Days) + 1;
    }

    public IEnumerator<HijriDate> GetEnumerator() => GetDays().GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
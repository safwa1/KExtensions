using KExtensions.Types;
using KExtensions.Utils;

namespace KExtensions;

public static class HijriDateExtensions
{
    public static DateTime ToMiladi(this HijriDate h) => h.ToDateTime();
    public static long ToUnixSeconds(this HijriDate h) => h.ToUnixTimeSeconds();

    // LINQ helpers: e.g., generate a HijriRange for a month
    public static HijriRange ToMonthRange(this HijriDate anyDayInMonth)
    {
        var start = new HijriDate(anyDayInMonth.Year, anyDayInMonth.Month, 1);
        // find number of days in month via calendar
        var dtStart = start.ToDateTime();
        var dtNextMonth = HijriDate.Calendar.AddMonths(dtStart, 1);
        var lastDay = HijriDate.Calendar.GetDayOfMonth(HijriDate.Calendar.AddMonths(dtStart, 1).AddDays(-1));
        var end = new HijriDate(anyDayInMonth.Year, anyDayInMonth.Month, lastDay);
        return new HijriRange(start, end);
    }
}
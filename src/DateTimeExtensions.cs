using KExtensions.Types;
using KExtensions.Utils;

namespace KExtensions;

public static class DateTimeExtensions
{
    public static HijriDate ToHijri(this DateTime dt) => HijriDate.FromDateTime(dt);
    public static HijriDate UnixSecondsToHijri(this long seconds) => HijriDate.FromUnixTimeSeconds(seconds);
    
    /// This extension method takes a start and end date and returns a list of those dates in an array.
    public static DateTime[] GetDatesArrayTo(this DateTime fromDate, DateTime toDate)
    {
        int days = (toDate - fromDate).Days;
        var dates = new DateTime[days];

        for (int i = 0; i < days; i++)
        {
            dates[i] = fromDate.AddDays(i);
        }

        return dates;
    }
}
namespace KExtensions;

public static class DateTimeExtensions
{
    private static string HijriOf(DateTime dateTime)
    {
        var hc = new System.Globalization.HijriCalendar();
        int year = hc.GetYear(dateTime);
        int month = hc.GetMonth(dateTime);
        int day = hc.GetDayOfMonth(dateTime);
        return $"{year}/{month:00}/{day:00}";
    }

    public static string ToHijriDate(this DateTime d)
    {
        var (year, month, day) = d;
        d = DateTime.Now;
        return HijriOf(d);
    }
    
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
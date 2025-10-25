using System;
using KExtensions;
using NUnit.Framework;

namespace KExtensions.Test;

[TestFixture]
public class StringsToDateTimeExtensionsTests
{
    [Test]
    public void ParseDate_DefaultFormat_dd_MM_yyyy()
    {
        var dt = "25/10/2025".ParseDate();
        Assert.That(dt.Year, Is.EqualTo(2025));
        Assert.That(dt.Month, Is.EqualTo(10));
        Assert.That(dt.Day, Is.EqualTo(25));
    }

    [Test]
    public void ParseTime_DefaultFormat_HH_mm_ss_EmptyDefaultsToMidnight()
    {
        var t1 = "23:59:58".ParseTime();
        Assert.That(t1.Hour, Is.EqualTo(23));
        Assert.That(t1.Minute, Is.EqualTo(59));
        Assert.That(t1.Second, Is.EqualTo(58));

        var t2 = string.Empty.ParseTime();
        Assert.That(t2.Hour, Is.EqualTo(0));
        Assert.That(t2.Minute, Is.EqualTo(0));
        Assert.That(t2.Second, Is.EqualTo(0));

        var t3 = "   ".ParseTime();
        Assert.That(t3.Hour, Is.EqualTo(0));
        Assert.That(t3.Minute, Is.EqualTo(0));
        Assert.That(t3.Second, Is.EqualTo(0));
    }

    [Test]
    public void ParseDateTime_DefaultFormat_dd_MM_yyyy_HH_mm_ss()
    {
        var dt = "25/10/2025 01:02:03".ParseDateTime();
        Assert.That(dt, Is.EqualTo(new DateTime(2025, 10, 25, 1, 2, 3)));
    }

    [Test]
    public void ParseExactInvariant_CustomFormat()
    {
        var dt = "2025-10-25".ParseExactInvariant("yyyy-MM-dd");
        Assert.That(dt, Is.EqualTo(new DateTime(2025, 10, 25)));
        Assert.Throws<FormatException>(() => "25-10-2025".ParseExactInvariant("yyyy-MM-dd"));
    }

    [Test]
    public void ParseDate_Invalid_Throws()
    {
        Assert.Throws<FormatException>(() => "2025/10/25".ParseDate());
    }
}

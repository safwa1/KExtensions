using System;
using System.Data;
using KExtensions;
using NUnit.Framework;

namespace KExtensions.Test;

[TestFixture]
public class CollectionsExtensionsTests
{
    private static readonly string[] Expected = ["@Id", "@Name"];

    [Test]
    public void ToSqlParams_Works_With_Nulls()
    {
        string[]? names = ["Id", "Name"];
        Assert.That(names.ToSqlParams(), Is.EqualTo(Expected));
        names = null;
        Assert.That(names.ToSqlParams(), Is.Empty);
    }

    [Test]
    public void ForEach_And_ForEachIndexed_OnArray()
    {
        int sum = 0;
        var arr = new[] {1,2,3};
        arr.ForEach(x => sum += x);
        Assert.That(sum, Is.EqualTo(6));

        int indexedSum = 0;
        arr.ForEachIndexed((i, x) => indexedSum += i * x);
        Assert.That(indexedSum, Is.EqualTo(0*1 + 1*2 + 2*3));
    }

    [Test]
    public void ForEach_And_ForEachIndexed_OnSpan()
    {
        int sum = 0;
        var arr = new[] {4,5,6};
        var span = arr.AsSpan();
        span.ForEach(x => sum += x);
        Assert.That(sum, Is.EqualTo(15));

        int indexedSum = 0;
        span.ForEachIndexed((i, x) => indexedSum += i + x);
        Assert.That(indexedSum, Is.EqualTo((0+4)+(1+5)+(2+6)));
    }

    private sealed class Person
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }

    [Test]
    public void ToDataTable_Produces_Columns_And_Rows()
    {
        var items = new[]
        {
            new Person { Id = 1, Name = "A" },
            new Person { Id = 2, Name = "B" }
        };

        DataTable table = items.ToDataTable();
        Assert.That(table.Columns.Count, Is.EqualTo(2));
        Assert.That(table.Columns[0].ColumnName, Is.EqualTo("Id"));
        Assert.That(table.Columns[1].ColumnName, Is.EqualTo("Name"));
        Assert.That(table.Rows.Count, Is.EqualTo(2));
        Assert.That(table.Rows[0]["Id"], Is.EqualTo(1));
        Assert.That(table.Rows[0]["Name"], Is.EqualTo("A"));
    }
}

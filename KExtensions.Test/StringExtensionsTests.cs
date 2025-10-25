using System.Text;

namespace KExtensions.Test;

[TestFixture]
public class StringExtensionsTests
{
    [Test]
    public void IsEmpty_Basic()
    {
        Assert.Multiple(() =>
        {
            Assert.That(string.Empty.IsEmpty(), Is.True);
            Assert.That("".IsEmpty(), Is.True);
            Assert.That("a".IsEmpty(), Is.False);
        });
        Assert.Throws<ArgumentNullException>(() => ((string)null!).IsEmpty());
    }

    [Test]
    public void IsNullOrEmpty_Variants()
    {
        Assert.Multiple(() =>
        {
            Assert.That(((string?)null).IsNullOrEmpty(), Is.True);
            Assert.That(string.Empty.IsNullOrEmpty(), Is.True);
            Assert.That("x".IsNullOrEmpty(), Is.False);
            Assert.That(((string?)null).IsNullOrWhiteSpace(), Is.True);
            Assert.That("   \t\n".IsNullOrWhiteSpace(), Is.True);
            Assert.That(" a ".IsNullOrWhiteSpace(), Is.False);
        });
    }

    [Test]
    public void Blank_Variants()
    {
        Assert.Multiple(() =>
        {
            Assert.That("   \t\n".IsBlank(), Is.True);
            Assert.That(" a ".IsBlank(), Is.False);
            Assert.That(((string?)null).IsNullOrBlank(), Is.True);
            Assert.That("   ".IsNullOrBlank(), Is.True);
            Assert.That("a".IsNullOrBlank(), Is.False);
            Assert.That("a".IsNotBlank(), Is.True);
        });
        Assert.Throws<ArgumentNullException>(() => ((string)null!).IsBlank());
    }

    [Test]
    public void OrEmpty_And_Or_Defaults()
    {
        string? n = null;
        Assert.Multiple(() =>
        {
            Assert.That(n.OrEmpty(), Is.EqualTo(string.Empty));
            Assert.That(n.Or("alt"), Is.EqualTo("alt"));
            Assert.That("x".Or("alt"), Is.EqualTo("x"));
        });
    }

    [Test]
    public void IfBlank_IfEmpty_Func_Defaults()
    {
        Assert.Multiple(() =>
        {
            Assert.That("   ".IfBlank(() => "alt"), Is.EqualTo("alt"));
            Assert.That("a".IfBlank(() => "alt"), Is.EqualTo("a"));
            Assert.That(string.Empty.IfEmpty(() => "alt"), Is.EqualTo("alt"));
            Assert.That("a".IfEmpty(() => "alt"), Is.EqualTo("a"));
        });
    }

    [Test]
    public void Remove_Substring()
    {
        Assert.Multiple(() =>
        {
            Assert.That("hello test world".Remove("test"), Is.EqualTo("hello  world"));
            // Remove removes the matched substring only (case-insensitive), leaving the rest intact
            Assert.That("Hello Xworld".Remove("xWOrld", ignoreCase: true), Is.EqualTo("Hello "));
            Assert.That("ab".Remove("z"), Is.EqualTo("ab"));
        });
    }

    [Test]
    public void Join_Ext()
    {
        var arr = new[] {"a","b","c"};
        Assert.Multiple(() =>
        {
            Assert.That(arr.Join(","), Is.EqualTo("a,b,c"));
            Assert.That(((string[]?)null).Join(","), Is.EqualTo(string.Empty));
        });
    }

    [Test]
    public void Drop_Take_Slice_Bounds()
    {
        Assert.Multiple(() =>
        {
            Assert.That("hello".Drop(3), Is.EqualTo("lo"));
            Assert.That("hello".DropLast(3), Is.EqualTo("he"));
            Assert.That("hello".Slice(0, 2), Is.EqualTo("he"));
            Assert.That("hello".Slice(1), Is.EqualTo("ello"));
        });
        Assert.Throws<IndexOutOfRangeException>(() => "".Drop(10));
        Assert.Throws<IndexOutOfRangeException>(() => "".DropLast(10));
        Assert.Multiple(() =>
        {
            Assert.That("hello".Take(2), Is.EqualTo("he"));
            Assert.That("hello".Take(2, 2), Is.EqualTo("ll"));
        });
    }

    [Test]
    public void SubstringHelpers_Before_After_Variants()
    {
        const string s = "prefix-MID-suffix";
        Assert.Multiple(() =>
        {
            Assert.That(s.SubstringBefore("MID"), Is.EqualTo("prefix-"));
            Assert.That(s.SubstringAfter("MID"), Is.EqualTo("-suffix"));
            Assert.That(s.SubstringBeforeLast("suf"), Is.EqualTo("prefix-MID-"));
            Assert.That(s.SubstringAfterLast("suf"), Is.EqualTo("fix"));
            Assert.That((null as string).SubstringBefore("x", "default"), Is.EqualTo("default"));
            Assert.That((null as string).SubstringAfter("x", "default"), Is.EqualTo("default"));
            Assert.That(s.SubstringBefore("zzz", "default"), Is.EqualTo("default"));
            Assert.That(s.SubstringAfter("zzz", "default"), Is.EqualTo("default"));
        });
    }

    [Test]
    public void Case_Conversions()
    {
        Assert.Multiple(() =>
        {
            Assert.That("HelloWorld".ToKebabCase(), Is.EqualTo("hello-world"));
            Assert.That("helloWorld".ToKebabCase(), Is.EqualTo("hello-world"));
            Assert.That("hello-world".ToCamelCase(), Is.EqualTo("helloWorld"));
            Assert.That(string.Empty.ToKebabCase(), Is.EqualTo(string.Empty));
            Assert.That(string.Empty.ToCamelCase(), Is.EqualTo(string.Empty));
        });
    }

    [Test]
    public void Capitalize_Decapitalize_Basics()
    {
        Assert.Multiple(() =>
        {
            Assert.That(("hello").Capitalize(), Is.EqualTo("Hello"));
            Assert.That(("HELLO").Decapitalize(), Is.EqualTo("hELLO"));
            Assert.That(((string?)null).Capitalize(), Is.EqualTo(string.Empty));
            Assert.That(((string?)null).Decapitalize(), Is.EqualTo(string.Empty));
        });
    }

    [Test]
    public void Bytes_And_Base64()
    {
        var bytes = "abc".ToByteArray(Encoding.ASCII);
        Assert.That(bytes, Is.EqualTo("abc"u8.ToArray()));
        var base64 = Convert.ToBase64String(bytes);
        Assert.Multiple(() =>
        {
            Assert.That(base64.Base64StringToByteArray(), Is.EqualTo(bytes));
            Assert.That(((string?)null).ToByteArray(), Is.EqualTo(Array.Empty<byte>()));
            Assert.That(((string?)null).Base64StringToByteArray(), Is.EqualTo(Array.Empty<byte>()));
        });
    }

    [Test]
    public void WordCount_And_Similarity_Sanity()
    {
        Assert.Multiple(() =>
        {
            Assert.That("hello world".WordCount(), Is.EqualTo(2));
            // sanity – identical strings should be 1.0 similarity
            // StringMatchPercentage returns percentage [0..100]
            Assert.That("same".StringMatchPercentage("same"), Is.EqualTo(100.0).Within(1e-9));
            Assert.That("abc".MatchSimilarity("abcd"), Is.InRange(0.0, 100.0));
        });
    }
}

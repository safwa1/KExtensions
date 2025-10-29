using KExtensions.Strings;

namespace KExtensions.Test;

[TestFixture]
public class StringsToNumbersExtensionsTests
{
    [Test]
    public void ParseCurrency_Strict_Valid_Invalid()
    {
        // InvariantCulture with NumberStyles.Currency does not accept $ or , separators
        Assert.Throws<FormatException>(() => "$1,234.56".ParseCurrency());
        Assert.That("-987.65".ParseCurrency(), Is.EqualTo(-987.65m));
        Assert.Throws<FormatException>(() => "abc".ParseCurrency());
        Assert.Throws<ArgumentNullException>(() => ((string)null!).ParseCurrency());
    }

    [Test]
    public void ParseCurrency_Loose_Normalizes()
    {
        Assert.That("USD 1 234,56".ParseCurrencyLoose(), Is.EqualTo(1234.56m));
        // Dots are treated as decimal separator in the current implementation, so "1.000" => 1m
        Assert.That("1.000".ParseCurrencyLoose(), Is.EqualTo(1m));
    }

    [Test]
    public void ParseDecimalInvariant_And_Try()
    {
        Assert.Multiple(() =>
        {
            Assert.That("12.5".ParseDecimalInvariant(), Is.EqualTo(12.5m));
            Assert.That("12,5".ParseDecimalInvariant(), Is.EqualTo(12.5m));
            Assert.That("12,5".TryDecimalInvariant(), Is.EqualTo(12.5m));
            Assert.That("abc".TryDecimalInvariant(), Is.Null);
        });
        Assert.Throws<ArgumentNullException>(() => ((string)null!).ParseDecimalInvariant());
    }

    [Test]
    public void ToNumeric_Primitives()
    {
        Assert.Multiple(() =>
        {
            Assert.That("42".ToInt(), Is.EqualTo(42));
            Assert.That("42".ToLong(), Is.EqualTo(42L));
            Assert.That("3.14".ToDouble(), Is.EqualTo(3.14).Within(1e-9));
            Assert.That("2.5".ToFloat(), Is.EqualTo(2.5f).Within(1e-6));
        });
        Assert.Throws<FormatException>(() => "x".ToDecimal());
        Assert.Throws<ArgumentNullException>(() => ((string)null!).ToInt());
    }
}

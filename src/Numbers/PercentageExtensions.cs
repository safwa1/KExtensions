using System.Numerics;

namespace KExtensions.Numbers;

public static class PercentageExtensions
{
    public static T CalculatePercentage<T>(this T number, T percentage)
        where T : INumber<T>
    {
        if (percentage < T.Zero || percentage > T.One)
            throw new ArgumentException("Percentage should be between 0 and 1.");

        return number * percentage;
    }

    public static T CalculatePercentage<T>(this T number, int percent)
        where T : INumber<T>
    {
        if (percent < 0 || percent > 100)
            throw new ArgumentException("Percentage should be between 0 and 100.");

        return number * T.CreateChecked(percent) / T.CreateChecked(100);
    }
}
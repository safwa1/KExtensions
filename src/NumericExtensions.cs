using System.Numerics;

namespace KExtensions;

public static class NumericExtensions
{
    public static bool ToBoolean(this int value)
    {
        return value switch
        {
            0 => false,
            1 => true,
            _ => throw new ArgumentOutOfRangeException(nameof(value), $"Value {value} cannot be converted to boolean.")
        };
    }

    public static int ToInt(this bool value) => value ? 1 : 0;

    public static T ToNegative<T>(this T value) where T : INumber<T>
    {
        return value > T.Zero ? -value : value;
    }

    public static T ToPositive<T>(this T value) where T : INumber<T>
    {
        return value < T.Zero ? T.Abs(value) : value;
    }

    public static T Increment<T>(this T number, T amount = default) where T : INumber<T>
    {
        return number + (amount == default ? T.One : amount);
    }

    public static T Decrement<T>(this T number, T amount = default) where T : INumber<T>
    {
        return number - (amount == default ? T.One : amount);
    }
}
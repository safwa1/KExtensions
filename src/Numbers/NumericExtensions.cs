using System.Numerics;
using System.Runtime.CompilerServices;

namespace KExtensions.Numbers;

public static class NumericExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ToBoolean<T>(this T value) where T : INumber<T>
    {
        return value switch
        {
            0 => false,
            1 => true,
            _ => throw new ArgumentOutOfRangeException(nameof(value), $"Value {value} cannot be converted to boolean.")
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ToInt(this bool value) => value ? 1 : 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T ToNegative<T>(this T value) where T : INumber<T>
    {
        return value > T.Zero ? -value : value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T ToPositive<T>(this T value) where T : INumber<T>
    {
        return value < T.Zero ? T.Abs(value) : value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Increment<T>(this T number, T? amount = default) where T : INumber<T>
    {
        return number + (amount == default || amount == null ? T.One : amount);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Decrement<T>(this T number, T? amount = default) where T : INumber<T>
    {
        return number - (amount == default || amount == null ? T.One : amount);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasFloatingPoint(this decimal number)
    {
        return number != Math.Truncate(number);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasFloatingPoint(this double number, double tolerance = 1e-10)
    {
        return Math.Abs(number - Math.Round(number)) > tolerance;
    }
}
using System.Runtime.CompilerServices;

namespace KExtensions;

public static class DecimalExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasFloatingPoint(this decimal number)
    {
        return number != Math.Truncate(number);
    }
}
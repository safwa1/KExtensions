using System.Runtime.CompilerServices;

namespace KExtensions;

public static class BoolExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T? TakeIf<T>(this T value, Func<T, bool> predicate) where T : class =>
        predicate(value) ? value : null;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T? TakeUnless<T>(this T value ,Func<T, bool> predicate) where T : class =>
        !predicate(value) ? value : null;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void IfTrue(this bool condition, Action action)
    {
        if (condition) action();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void IfFalse(this bool condition, Action action)
    {
        if (!condition) action();
    }
}

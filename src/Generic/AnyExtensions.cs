using System.Runtime.CompilerServices;

namespace KExtensions.Generic;

public static class AnyExtensions
{
    /// <summary>
    /// Kotlin's "run" - Calls the specified function with 'this' value as its receiver and returns its result.
    /// Use when you want to execute a lambda on an object and return the lambda result.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TResult Run<T, TResult>(this T obj, Func<T, TResult> block)
    {
        return block(obj);
    }

    /// <summary>
    /// Kotlin's "run" (void version) - Calls the specified action with 'this' value as its receiver.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Run<T>(this T obj, Action<T> block)
    {
        block(obj);
    }

    /// <summary>
    /// Kotlin's "let" - Calls the specified function with 'this' value as its argument and returns its result.
    /// Use for null checking and transforming values.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TResult? Let<T, TResult>(this T? obj, Func<T, TResult> block)
        where T : notnull
    {
        return obj is null ? default : block(obj);
    }

    /// <summary>
    /// Kotlin's "let" (void version) - Calls the specified action with 'this' value as its argument.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Let<T>(this T? obj, Action<T> block)
        where T : notnull
    {
        if (obj is not null)
            block(obj);
    }

    /// <summary>
    /// Kotlin's "apply" - Calls the specified function with 'this' value as its receiver and returns 'this' value.
    /// Use for object configuration/initialization.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Apply<T>(this T obj, Action<T> block)
    {
        block(obj);
        return obj;
    }

    /// <summary>
    /// Kotlin's "also" - Calls the specified function with 'this' value as its argument and returns 'this' value.
    /// Use for side effects while keeping the original object in the chain.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Also<T>(this T obj, Action<T> block)
    {
        block(obj);
        return obj;
    }

    /// <summary>
    /// Kotlin's "takeIf" - Returns 'this' value if it satisfies the given predicate, otherwise returns null.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T? TakeIf<T>(this T obj, Func<T, bool> predicate) where T : notnull
    {
        return predicate(obj) ? obj : default;
    }

    /// <summary>
    /// Kotlin's "takeUnless" - Returns 'this' value if it does NOT satisfy the given predicate, otherwise returns null.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T? TakeUnless<T>(this T obj, Func<T, bool> predicate) where T : notnull
    {
        return predicate(obj) ? default : obj;
    }
}
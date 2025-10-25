using System.Runtime.CompilerServices;

namespace KExtensions;

public static class ListExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<T> TakeUnlessEmpty<T>(this IEnumerable<T> source)
    {
        var empty = source as T[] ?? [.. source];
        return empty.Length != 0 ? empty : [];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<T> FilterNot<T>(this IEnumerable<T> source, Func<T, bool> predicate) =>
        source.Where(x => !predicate(x));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<TResult> MapNotNull<T, TResult>(this IEnumerable<T> source, Func<T, TResult?> selector)
        where TResult : class =>
        source.Select(selector).Where(x => x != null)!;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ForEachIndexed<T>(this IEnumerable<T> source, Action<int, T> action)
    {
        int index = 0;
        foreach (var item in source)
            action(index++, item);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector)
    {
        var seen = new HashSet<TKey>();
        foreach (var item in source)
            if (seen.Add(keySelector(item))) yield return item;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEmpty<T>(this List<T> list)
    {
        return list.Count == 0;
    }    
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEmpty<T>(this IEnumerable<T> source)
    {
        return !source.Any();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEmpty<T>(this T[] list)
    {
        return list.Length == 0;
    }
}
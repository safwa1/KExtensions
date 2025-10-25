using System.Collections.Concurrent;

namespace KExtensions;

public static class DictListExtensions
{
    public static void AddToCollection<TKey, TValue, TCollection>(
        this Dictionary<TKey, TCollection> dict, TKey key, TValue value)
        where TCollection : ICollection<TValue>, new() where TKey : notnull
    {
        if (!dict.TryGetValue(key, out var collection))
        {
            collection = new TCollection();
            dict.Add(key, collection);
        }
        collection.Add(value);
    }

    public static bool RemoveFromCollection<TKey, TValue, TCollection>(
        this Dictionary<TKey, TCollection> dict, TKey key, TValue value)
        where TCollection : ICollection<TValue> where TKey : notnull
    {
        if (!dict.TryGetValue(key, out var collection)) return false;
        bool removed = collection.Remove(value);
        if (collection.Count == 0) dict.Remove(key);
        return removed;
    }
        
    public static bool CompareLists<T>(this IReadOnlyList<T>? list1, IReadOnlyList<T>? list2, bool isOrdered = true)
    {
        if (list1 == null && list2 == null) return true;
        if (list1 == null || list2 == null || list1.Count != list2.Count) return false;

        if (isOrdered)
        {
            for (int i = 0; i < list1.Count; i++)
            {
                if (!EqualityComparer<T>.Default.Equals(list1[i], list2[i])) return false;
            }
            return true;
        }

        // O(n) unordered comparison using dictionary
        var counts = new Dictionary<T, int>(EqualityComparer<T>.Default);
        foreach (var item in list2)
        {
            if (!counts.TryAdd(item, 1)) counts[item]++;
        }

        foreach (var item in list1)
        {
            if (!counts.TryGetValue(item, out var c) || c == 0) return false;
            counts[item] = c - 1;
        }

        return true;
    }

    // ---------------- Strings ----------------


    // ---------------- Nullable Type Extensions ----------------
    public static bool IsNullable(this Type? type) => type != null && Nullable.GetUnderlyingType(type) != null;

    public static bool IsExactOrNullable<T>(this Type? type) where T : struct
    {
        if (type == null) return false;
        if (Nullable.GetUnderlyingType(type) is Type underlying) type = underlying;
        return type == typeof(T);
    }

    public static Type ToNullable(this Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (type.IsNullable()) return type;
        return NullableTypesCache.Get(type);
    }
        
    public static bool ContainsSequence<T>(this T[]? array, T[]? candidate)
    {
        if (array == null || candidate == null || candidate.Length == 0 || candidate.Length > array.Length)
            return false;

        for (int i = 0; i <= array.Length - candidate.Length; i++)
        {
            bool match = true;
            for (int j = 0; j < candidate.Length; j++)
            {
                if (!EqualityComparer<T>.Default.Equals(array[i + j], candidate[j]))
                {
                    match = false;
                    break;
                }
            }

            if (match) return true;
        }

        return false;
    }
}
    
public static class NullableTypesCache
{
    private static readonly ConcurrentDictionary<Type, Type> Cache = new();

    private static readonly Type NullableBase = typeof(Nullable<>);

    static NullableTypesCache()
    {
        foreach (var t in new[]
                 {
                     typeof(byte), typeof(short), typeof(int), typeof(long),
                     typeof(float), typeof(double), typeof(decimal),
                     typeof(sbyte), typeof(ushort), typeof(uint), typeof(ulong)
                 })
        {
            Cache.TryAdd(t, NullableBase.MakeGenericType(t));
        }
    }

    internal static Type Get(Type type) => Cache.GetOrAdd(type, t => NullableBase.MakeGenericType(t));
}
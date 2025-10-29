using System.ComponentModel;
using System.Data;
using System.Runtime.CompilerServices;

namespace KExtensions.Collections;

public static class CollectionsExtensions
{
    private const int StackLimit = 1024;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T[] Take<T>(this T[] array, int count) where T : unmanaged
    {
        if (array == null) throw new ArgumentNullException(nameof(array));
        if (count <= 0) return [];

        int n = Math.Min(count, array.Length);

        if (n <= StackLimit)
        {
            Span<T> buffer = stackalloc T[n];
            array.AsSpan(0, n).CopyTo(buffer);
            return buffer.ToArray();
        }

        var result = new T[n];
        Array.Copy(array, 0, result, 0, n);
        return result;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<T> Take<T>(this IEnumerable<T> source, int count)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (count <= 0) yield break;

        int i = 0;
        foreach (var item in source)
        {
            if (i++ >= count) yield break;
            yield return item;
        }
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<T> TakeLast<T>(this T[] array, int count)
    {
        if (array == null) throw new ArgumentNullException(nameof(array));
        if (count <= 0) return Span<T>.Empty;

        int n = Math.Min(count, array.Length);
        int start = array.Length - n;

        return array.AsSpan(start, n);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> source, int count)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (count <= 0) yield break;

        if (source is IList<T> list)
        {
            int start = Math.Max(0, list.Count - count);
            for (int i = start; i < list.Count; i++)
                yield return list[i];
        }
        else
        {
            Queue<T> queue = new Queue<T>(count);
            foreach (var item in source)
            {
                if (queue.Count == count)
                    queue.Dequeue();
                queue.Enqueue(item);
            }

            foreach (var item in queue)
                yield return item;
        }
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<T> Drop<T>(this T[] array, int count)
    {
        if (array == null) throw new ArgumentNullException(nameof(array));
        int n = Math.Min(array.Length, Math.Max(0, count));
        return array.AsSpan(n);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<T> Drop<T>(this IEnumerable<T>? source, int count)
    {
        if (source == null) yield break;
        if (count <= 0)
        {
            foreach (var item in source) yield return item;
            yield break;
        }

        if (source is IList<T> list)
        {
            for (int i = count; i < list.Count; i++)
                yield return list[i];
        }
        else
        {
            int i = 0;
            foreach (var item in source)
            {
                if (i++ >= count) yield return item;
            }
        }
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<T> DropLast<T>(this T[] array, int count)
    {
        if (array == null) throw new ArgumentNullException(nameof(array));
        int n = Math.Max(0, array.Length - count);
        return array.AsSpan(0, n);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<T> DropLast<T>(this IEnumerable<T> source, int count)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (count <= 0)
        {
            foreach (var item in source) yield return item;
            yield break;
        }

        if (source is IList<T> list)
        {
            int end = Math.Max(0, list.Count - count);
            for (int i = 0; i < end; i++)
                yield return list[i];
        }
        else
        {
            Queue<T> queue = new Queue<T>(count);
            foreach (var item in source)
            {
                if (queue.Count == count)
                    yield return queue.Dequeue();
                queue.Enqueue(item);
            }

            // remaining items in queue are dropped
        }
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<T> Reverse<T>(this IList<T> list)
    {
        if (list == null) throw new ArgumentNullException(nameof(list));
        for (int i = list.Count - 1; i >= 0; i--)
            yield return list[i];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<T> Reverse<T>(this IEnumerable<T> source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        var array = source as T[] ?? source.ToArray();
        for (int i = array.Length - 1; i >= 0; i--)
            yield return array[i];
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<List<T>> Batch<T>(this IEnumerable<T> source, int batchSize)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (batchSize <= 0) throw new ArgumentOutOfRangeException(nameof(batchSize));

        var batch = new List<T>(batchSize);
        foreach (var item in source)
        {
            batch.Add(item);
            if (batch.Count == batchSize)
            {
                yield return batch;
                batch = new List<T>(batchSize);
            }
        }

        if (batch.Count > 0) yield return batch;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string[] ToSqlParams(this string[]? self)
    {
        if (self == null) return [];
        
        string[] result = new string[self.Length];

        for (var i = 0; i < self.Length; i++)
        {
            result[i] = $"@{self[i]}";
        }
        
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ForEach<T>(this T[] array, Action<T> action)
    {
        foreach (var element in array)
        {
            action(element);
        }
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ForEach<T>(this Span<T> span, Action<T> action)
    {
        foreach (var element in span)
        {
            action(element);
        }
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ForEachIndexed<T>(this T[] array, Action<int, T> action)
    {
        for (int i = 0; i < array.Length; i++)
        {
            action(i, array[i]);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ForEachIndexed<T>(this Span<T> span, Action<int, T> action)
    {
        for (int i = 0; i < span.Length; i++)
        {
            action(i, span[i]);
        }
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DataTable ToDataTable<T>(this IEnumerable<T> data)
    {
        PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
        DataTable table = new DataTable();
        for (int i = 0; i < props.Count; i++)
        {
            PropertyDescriptor prop = props[i];
            table.Columns.Add(prop.Name, prop.PropertyType);
        }
        object?[] values = new object?[props.Count];
        foreach (T item in data)
        {
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = props[i].GetValue(item);
            }
            table.Rows.Add(values);
        }
        return table;
    }
    
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
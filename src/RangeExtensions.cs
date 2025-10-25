using KExtensions.Utils;

namespace KExtensions;

public static class RangeExtensions {

    public static  CustomIntEnumerator GetEnumerator(this Range range)
    {
        return new CustomIntEnumerator(range);
    }

    public static  CustomIntEnumerator GetEnumerator(this int number)
    {
        return new CustomIntEnumerator(new Range(0, number));
    }
    
    public static bool All(this Range range, Func<int, bool> func)
    {
        int start = range.Start.Value;
        int end = range.End.Value;
        for (int i = start; i < end; i++)
        {
            if (!func(i))
                return false;
        }

        return true;
    }
    
    public static void ForEach(this Range range, Action<int> callback)
    {
        int start = range.Start.Value;
        int end = range.End.Value;
        for (int i = start; i <= end; i++)
        {
            callback?.Invoke(i);
        }
    }

    public static bool Any(this Range range, Func<int, bool> func)
    {
        int start = range.Start.Value;
        int end = range.End.Value;
        for (int i = start; i < end; i++)
        {
            if (func(i))
                return true;
        }

        return false;
    }

    public static IEnumerable<int> Iterator(this Range range)
    {
        int start = range.Start.Value;
        int end = range.End.Value;

        // Ensure the range works correctly with Enumerable.Range
        int count = end - start;

        if (count < 0)
            throw new ArgumentException("End value must be greater than or equal to start value.");
        
        return Enumerable.Range(start, end);
    }

    public static int Sum(this Range range)
    {
        return range.Iterator().Sum();
    }
    
    public static int Single(this Range range)
    {
        return range.Iterator().Single();
    }
    
    public static int? SingleOrDefault(this Range range)
    {
        return range.Iterator().SingleOrDefault();
    }
    
    public static IEnumerable<int> Skip(this Range range, int count)
    {
        return range.Iterator().Skip(count);
    }
    
    public static IEnumerable<int> SkipLast(this Range range, int count)
    {
        return range.Iterator().SkipLast(count);
    }
    
    public static IEnumerable<int> SkipWhile(this Range range, Func<int, bool> predicate)
    {
        return range.Iterator().SkipWhile(predicate);
    }

    public static bool LinqAll(this Range range, Func<int, bool> predicate)
    {
        return range.Iterator().All(predicate);
    }
    
    public static IEnumerable<TResult> Select<TResult>(this Range range, Func<int, TResult> selector)
    {
        int start = range.Start.Value;
        int end = range.End.Value;

        // Ensure the range works correctly with Enumerable.Range
        int count = end - start;

        if (count < 0)
            throw new ArgumentException("End value must be greater than or equal to start value.");

        return Enumerable.Range(start, end).Select(selector);
    }

}
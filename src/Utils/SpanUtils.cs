namespace KExtensions.Utils;

public static class SpanUtils
{
    
    public static T[] Concat<T>(ReadOnlySpan<T> span)
    {
        var result = new T[span.Length];
        span.CopyTo(result);
        return result;
    }

    public static T[] Concat<T>(ReadOnlySpan<T> span0, ReadOnlySpan<T> span1)
    {
        var result = new T[span0.Length + span1.Length];
        var resultSpan = result.AsSpan();
        span0.CopyTo(result);
        var from = span0.Length;
        span1.CopyTo(resultSpan.Slice(from));
        return result;
    }

    public static T[] Concat<T>(ReadOnlySpan<T> span0, ReadOnlySpan<T> span1, ReadOnlySpan<T> span2)
    {
        var result = new T[span0.Length + span1.Length + span2.Length];
        var resultSpan = result.AsSpan();
        span0.CopyTo(result);
        var from = span0.Length;
        span1.CopyTo(resultSpan.Slice(from));
        from += span1.Length;
        span2.CopyTo(resultSpan.Slice(from));
        return result;
    }

    public static T[] Concat<T>(ReadOnlySpan<T> span0, ReadOnlySpan<T> span1, ReadOnlySpan<T> span2,
        ReadOnlySpan<T> span3)
    {
        var result = new T[span0.Length + span1.Length + span2.Length + span3.Length];
        var resultSpan = result.AsSpan();
        span0.CopyTo(result);
        var from = span0.Length;
        span1.CopyTo(resultSpan.Slice(from));
        from += span1.Length;
        span2.CopyTo(resultSpan.Slice(from));
        from += span2.Length;
        span3.CopyTo(resultSpan.Slice(from));
        return result;
    }

    public static T[] Concat<T>(ReadOnlySpan<T> span0, ReadOnlySpan<T> span1, ReadOnlySpan<T> span2,
        ReadOnlySpan<T> span3, ReadOnlySpan<T> span4)
    {
        var result = new T[span0.Length + span1.Length + span2.Length + span3.Length + span4.Length];
        var resultSpan = result.AsSpan();
        span0.CopyTo(result);
        var from = span0.Length;
        span1.CopyTo(resultSpan.Slice(from));
        from += span1.Length;
        span2.CopyTo(resultSpan.Slice(from));
        from += span2.Length;
        span3.CopyTo(resultSpan.Slice(from));
        from += span3.Length;
        span4.CopyTo(resultSpan.Slice(from));
        return result;
    }
}


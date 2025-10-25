using System.Runtime.CompilerServices;
using KExtensions.Utils;

namespace KExtensions;

public static class SpanExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T[] Concat<T>(this ReadOnlySpan<T> span0, ReadOnlySpan<T> span1)
        => SpanUtils.Concat(span0, span1);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T[] Concat<T>(this ReadOnlySpan<T> span0, ReadOnlySpan<T> span1, ReadOnlySpan<T> span2)
        => SpanUtils.Concat(span0, span1, span2);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T[] Concat<T>(this ReadOnlySpan<T> span0, ReadOnlySpan<T> span1, ReadOnlySpan<T> span2,
        ReadOnlySpan<T> span3)
        => SpanUtils.Concat(span0, span1, span2, span3);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T[] Concat<T>(this ReadOnlySpan<T> span0, ReadOnlySpan<T> span1, ReadOnlySpan<T> span2,
        ReadOnlySpan<T> span3, ReadOnlySpan<T> span4)
        => SpanUtils.Concat(span0, span1, span2, span3, span4);
}
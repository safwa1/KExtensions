using System.Runtime.CompilerServices;
using System.Text;

namespace KExtensions;

public static class JoinExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string JoinToString<T>(
        this IEnumerable<T>? source,
        string separator = ", ",
        string prefix = "",
        string postfix = "",
        int limit = -1,
        string truncated = "...",
        Func<T, string>? transform = null)
    {
        if (source == null) return string.Empty;

        var sb = new StringBuilder();
        sb.Append(prefix);

        int count = 0;
        foreach (var item in source)
        {
            if (limit >= 0 && count >= limit)
            {
                sb.Append(truncated);
                break;
            }

            if (count > 0)
                sb.Append(separator);

            sb.Append(transform?.Invoke(item) ?? item?.ToString());
            count++;
        }

        sb.Append(postfix);
        return sb.ToString();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string JoinToString<T>(this T[] array, string separator = ", ",
        string prefix = "", string postfix = "", int limit = -1,
        string truncated = "...", Func<T, string>? transform = null)
    {
        return ((IEnumerable<T>)array).JoinToString(separator, prefix, postfix, limit, truncated, transform);
    }
}
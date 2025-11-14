using System.Runtime.CompilerServices;
using System.Text;

namespace KExtensions;

public static class ByteExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToBase64(this byte[] value)
    {
        return Convert.ToBase64String(value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string GetString(this byte[] value)
    {
        return Encoding.UTF8.GetString(value);
    }
}



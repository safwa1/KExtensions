using System.Runtime.CompilerServices;

namespace KExtensions.Numbers;

public static class SpeedAndSizeExtensions
{
    private const long KiloByte = 1024;
    private const long MegaByte = KiloByte * 1024;

    /// <summary>
    /// Formats speed in KB/s or MB/s.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string SpeedFormat(this long speedInBytes, bool inMbps = false)
    {
        double value = inMbps ? speedInBytes / (double)MegaByte : speedInBytes / (double)KiloByte;
        string unit = inMbps ? "MB/s" : "KB/s";
        return $"{value:0.00} {unit}";
    }

    /// <summary>
    /// Formats size in KB or MB.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string SizeFormat(this long sizeInBytes, bool inMB = true)
    {
        double value = inMB ? sizeInBytes / (double)MegaByte : sizeInBytes / (double)KiloByte;
        string unit = inMB ? "MB" : "KB";
        return $"{value:0.00} {unit}";
    }
}
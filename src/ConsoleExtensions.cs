using KExtensions.Utils;

namespace KExtensions;

public static class ConsoleExtensions
{
#if NET10_0_OR_GREATER
    extension(Console)
    {
        public static void Table<T>(T data) => ConsoleUtils.WriteTable<T>(data);
    }
#endif
}

namespace KExtensions.Utils;

public static class ConsoleUtils
{
    // Main method that handles all overloads
    public static void WriteTable<T>(T data)
    {
        if (data is IEnumerable<string> stringEnumerable)
        {
            WriteTable(["Value"], [.. stringEnumerable.Select(x => new[] { x })]);
        }
        else if (data is System.Collections.IEnumerable enumerable && !(data is string))
        {
            var elementType = GetEnumerableElementType(data.GetType());
            if (elementType == typeof(string))
            {
                // Handle List<string>, string[], etc.
                var stringItems = enumerable.Cast<object>().Select(x => x.ToString()).ToArray();
                WriteTable(["Value"], [.. stringItems.Select(x => new[] { x })]);
            }
            else if (elementType.IsClass && elementType != typeof(object))
            {
                // Handle List<SomeClass>
                var method = typeof(ConsoleUtils).GetMethod("WriteObjectTable");
                var generic = method?.MakeGenericMethod(elementType);
                generic?.Invoke(null, [enumerable]);
            }
            else
            {
                // Handle List<int>, List<double>, etc.
                var items = enumerable.Cast<object>().Select(x => x.ToString()).ToArray();
                WriteTable(["Value"], [.. items.Select(x => new[] { x })]);
            }
        }
        else
        {
            // Single value
            WriteTable(["Value"], [[data?.ToString()]]);
        }
    }

    // Handle object collections
    public static void WriteObjectTable<T>(IEnumerable<T> data)
    {
        var properties = typeof(T).GetProperties();
        var headers = properties.Select(p => p.Name).ToArray();
        var rows = data.Select(item => 
            properties.Select(p => 
            {
                var value = p.GetValue(item);
                return value?.ToString() ?? "null";
            }).ToArray()
        ).ToArray();

        WriteTable(headers, rows);
    }

    // Explicit table with headers and rows
    public static void WriteTable(string[] headers, IEnumerable<IEnumerable<object>> data)
    {
        var rows = data.Select(row => row.Select(x => x.ToString() ?? "null").ToArray()).ToArray();
        WriteTable(headers, rows);
    }

    private static void WriteTable(string[] headers, string?[][] rows)
    {
        if (headers == null || headers.Length == 0)
            throw new ArgumentException("Headers cannot be null or empty");

        // Calculate column widths
        int[] columnWidths = headers.Select((h, i) => 
            Math.Max(
                h.Length,
                rows.Length != 0 ? rows.Max(r =>
                {
                    if (i >= 0 && i < r.Length) return i < r.Length ? r[i]!.Length : 0;
                    return 0;
                }) : 0
            )
        ).ToArray();

        // Build horizontal border
        var horizontalBorder = "+" + string.Join("+", 
            columnWidths.Select(w => new string('-', w + 2))
        ) + "+";

        // Build format string
        var format = "| " + string.Join(" | ", 
            columnWidths.Select((w, i) => "{" + i + ",-" + w + "}")
        ) + " |";

        // Print top border
        Console.WriteLine(horizontalBorder);
        
        // Print header
        Console.WriteLine(format, headers);
        
        // Print header separator
        Console.WriteLine(horizontalBorder);

        // Print rows
        foreach (var row in rows)
        {
            var formattedRow = new object?[headers.Length];
            for (int i = 0; i < headers.Length; i++)
            {
                formattedRow[i] = i < row.Length ? row[i] : string.Empty;
            }
            Console.WriteLine(format, formattedRow);
        }

        // Print bottom border
        Console.WriteLine(horizontalBorder);
    }

    private static Type GetEnumerableElementType(Type type)
    {
        if (type.IsArray)
            return type.GetElementType()!;

        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            return type.GetGenericArguments()[0];

        var iface = type.GetInterfaces()
            .FirstOrDefault(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>));

        return iface?.GetGenericArguments()[0] ?? typeof(object);
    }
}
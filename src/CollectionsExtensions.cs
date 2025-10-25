using System.ComponentModel;
using System.Data;

namespace KExtensions;

public static class CollectionsExtensions
{
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

    public static void ForEach<T>(this T[] array, Action<T> action)
    {
        foreach (var element in array)
        {
            action(element);
        }
    }
    
    public static void ForEach<T>(this Span<T> span, Action<T> action)
    {
        foreach (var element in span)
        {
            action(element);
        }
    }
    
    public static void ForEachIndexed<T>(this T[] array, Action<int, T> action)
    {
        for (int i = 0; i < array.Length; i++)
        {
            action(i, array[i]);
        }
    }

    public static void ForEachIndexed<T>(this Span<T> span, Action<int, T> action)
    {
        for (int i = 0; i < span.Length; i++)
        {
            action(i, span[i]);
        }
    }
    
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
    
}